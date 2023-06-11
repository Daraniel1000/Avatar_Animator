using Assets.Scenes.FaceTracking;
using Assets.Scripts;
using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HandHelper
{
    private readonly BoneRot handRotation;
    private readonly BonePos[] fingers;
    private readonly GameObject armRoot;
    private readonly BonePos armTarget, armHint;

    private readonly float scaleY = -1f, scaleFinger = 1f;
    private readonly string hand;
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
            new BonePos($"{hand}Index Target1", mincutoff, beta),
            new BonePos($"{hand}Index Target2", mincutoff, beta),
            new BonePos($"{hand}Middle Target", mincutoff, beta),
            new BonePos($"{hand}Middle Target1", mincutoff, beta),
            new BonePos($"{hand}Middle Target2", mincutoff, beta),
            new BonePos($"{hand}Ring Target", mincutoff, beta),
            new BonePos($"{hand}Ring Target1", mincutoff, beta),
            new BonePos($"{hand}Ring Target2", mincutoff, beta),
            new BonePos($"{hand}Pinky Target", mincutoff, beta),
            new BonePos($"{hand}Pinky Target1", mincutoff, beta),
            new BonePos($"{hand}Pinky Target2", mincutoff, beta),
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
    }

    public void HandleHandUpdate(List<Vec3> data, List<Vec3> body, int idx)
    {
        // Arm location rig
        armTarget.SetRelativePosition((body[armTargetIdx] - body[armRootIdx]).ToVector().scaleY(scaleY), armRoot.transform.position);
        armHint.SetRelativePosition((body[armHintIdx] - body[armRootIdx]).ToVector().scaleY(scaleY), armRoot.transform.position);

        var rot = GetHandLookVectors(data, idx);
        handRotation.SetRotation(Quaternion.LookRotation(rot.Item1, rot.Item2));

        Vector3[] positions = new Vector3[]
        {
            (data[4] - data[0]).ToVector().scaleY(scaleY),
            (data[6] - data[0]).ToVector().scaleY(scaleY),
            (data[7] - data[0]).ToVector().scaleY(scaleY),
            (data[8] - data[0]).ToVector().scaleY(scaleY),
            (data[10] - data[0]).ToVector().scaleY(scaleY),
            (data[11] - data[0]).ToVector().scaleY(scaleY),
            (data[12] - data[0]).ToVector().scaleY(scaleY),
            (data[14] - data[0]).ToVector().scaleY(scaleY),
            (data[15] - data[0]).ToVector().scaleY(scaleY),
            (data[16] - data[0]).ToVector().scaleY(scaleY),
            (data[18] - data[0]).ToVector().scaleY(scaleY),
            (data[19] - data[0]).ToVector().scaleY(scaleY),
            (data[20] - data[0]).ToVector().scaleY(scaleY),
        };
        Vector3 offset = armTarget.bone.transform.position;

        fingers[0].SetRelativePosition(positions[0], offset);
        for (int i = 1; i * 3 < fingers.Length; i++)
        {
            fingers[i * 3 - 2].SetRelativePosition(positions[i * 3 - 2] * scaleFinger, offset);
            fingers[i * 3 - 1].SetRelativePosition(positions[i * 3 - 1] * scaleFinger, offset);
            fingers[i * 3].SetRelativePosition(positions[i * 3] * scaleFinger + rot.Item1 * .01f, offset);
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
}