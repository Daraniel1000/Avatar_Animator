using Assets.Scenes.FaceTracking;
using Assets.Scripts;
using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HandHelper
{
    private readonly BoneRot handRotation;
    private readonly BonePos[] fingers;
    private readonly BoneRot[][] fingerRots;
    private readonly GameObject armRoot;
    private readonly BonePos armTarget, armHint;
    private readonly Finger[] fingerTransforms;

    private readonly float scaleY = -1f, scaleFinger = 1f;
    public readonly string hand;
    private readonly int armRootIdx, armTargetIdx, armHintIdx;

    private HandHelper() { }

    public HandHelper(string hand)
    {
        if (hand != "L" && hand != "R")
            throw new Exception("HandHelper must be given L or R");
        this.hand = hand;

        handRotation = new BoneRot($"{hand}H IK Rotator");
        float mincutoff = 0.8f, beta = 0.8f;
        fingers = new BonePos[]
        {
            new BonePos($"{hand}Thumb Target", mincutoff, beta),
            new BonePos($"{hand}Index Target", mincutoff, beta),
            new BonePos($"{hand}Middle Target", mincutoff, beta),
            new BonePos($"{hand}Ring Target", mincutoff, beta),
            new BonePos($"{hand}Pinky Target", mincutoff, beta),
        };
        fingerRots = new BoneRot[][]
        {
            null,
            new BoneRot[]
            {
                new BoneRot($"{hand}Index.001"),
                new BoneRot($"{hand}Index.002"),
            },
            new BoneRot[]
            {
                new BoneRot($"{hand}Middle.001"),
                new BoneRot($"{hand}Middle.002"),
            },
            new BoneRot[]
            {
                new BoneRot($"{hand}Ring.001"),
                new BoneRot($"{hand}Ring.002"),
            },
            new BoneRot[]
            {
                new BoneRot($"{hand}Pinky.001"),
                new BoneRot($"{hand}Pinky.002"),
            },
        };
        if (hand == "R")
        {
            armRootIdx = 12;
            armTargetIdx = 16;
            armHintIdx = 14;
        }
        else
        {
            armRootIdx = 11;
            armTargetIdx = 15;
            armHintIdx = 13;
        }
        armTarget = new BonePos($"{hand}H IK Target");
        armHint = new BonePos($"{hand}H IK Hint");
        armRoot = GameObject.Find($"Arm.{hand}");
        fingerTransforms = new Finger[]
        {
            new Finger($"{hand}Thumb"),
            new Finger($"{hand}Index.000"),
            new Finger($"{hand}Middle.000"),
            new Finger($"{hand}Ring.000"),
            new Finger($"{hand}Pinky.000"),
        };
    }

    public void HandleHandUpdate(List<Vec3> data, List<Vec3> body, int idx)
    {
        // Arm location rig
        armTarget.SetRelativePosition((body[armTargetIdx] - body[armRootIdx]).ToVector().scaleY(scaleY), armRoot.transform.position);
        armHint.SetRelativePosition((body[armHintIdx] - body[armRootIdx]).ToVector().scaleY(scaleY), armRoot.transform.position);

        //Hand
        var rot = GetHandLookVectors(data, idx);
        handRotation.SetRotation(Quaternion.LookRotation(rot.Item1, rot.Item2));

        //Fingers rig
        Vector3[] positions = new Vector3[]
        {
            (data[4] - data[0]).ToVector().scaleY(scaleY),
            (data[6] - data[0]).ToVector().scaleY(scaleY),
            (data[10] - data[0]).ToVector().scaleY(scaleY),
            (data[14] - data[0]).ToVector().scaleY(scaleY),
            (data[18] - data[0]).ToVector().scaleY(scaleY),
        };
        Vector3 offset = armTarget.bone.transform.position;

        fingerRots[1][0].SetXRotation(Vector3.Angle((data[7] - data[6]).ToVector().scaleY(scaleY), (data[6] - data[5]).ToVector().scaleY(scaleY)));
        fingerRots[1][1].SetXRotation(Vector3.Angle((data[8] - data[7]).ToVector().scaleY(scaleY), (data[7] - data[6]).ToVector().scaleY(scaleY)));
        fingerRots[2][0].SetXRotation(Vector3.Angle((data[11] - data[10]).ToVector().scaleY(scaleY), (data[10] - data[9]).ToVector().scaleY(scaleY)));
        fingerRots[2][1].SetXRotation(Vector3.Angle((data[12] - data[11]).ToVector().scaleY(scaleY), (data[11] - data[10]).ToVector().scaleY(scaleY)));
        fingerRots[3][0].SetXRotation(Vector3.Angle((data[15] - data[14]).ToVector().scaleY(scaleY), (data[14] - data[13]).ToVector().scaleY(scaleY)));
        fingerRots[3][1].SetXRotation(Vector3.Angle((data[16] - data[15]).ToVector().scaleY(scaleY), (data[15] - data[14]).ToVector().scaleY(scaleY)));
        fingerRots[4][0].SetXRotation(Vector3.Angle((data[19] - data[18]).ToVector().scaleY(scaleY), (data[18] - data[17]).ToVector().scaleY(scaleY)));
        fingerRots[4][1].SetXRotation(Vector3.Angle((data[20] - data[19]).ToVector().scaleY(scaleY), (data[19] - data[18]).ToVector().scaleY(scaleY)));

        fingers[0].SetRelativePosition(positions[0], offset);
        for (int i = 1; i < fingerRots.Length; i++)
        {
            fingers[i].SetRelativePosition(positions[i] * scaleFinger, offset);
        }
    }

    private Tuple<Vector3, Vector3> GetHandLookVectors(List<Vec3> data, int idx)
    {
        var points = new Vector3[] { data[0].ToVector().scaleY(scaleY), data[5].ToVector().scaleY(scaleY), data[17].ToVector().scaleY(scaleY) };
        var front = (idx == 0 ? 1 : -1) * Vector3.Cross(points[2] - points[0], points[1] - points[2]).normalized;
        var midpoint = Vector3.Lerp(points[1], points[2], .5f);
        var up = midpoint - points[0];
        up.Normalize();
        return new Tuple<Vector3, Vector3>(front, up);
    }

    public void resetFingerRotations()
    {
        fingerTransforms[2].constrainAngles();
    }
}