// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Resolvers
{
    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        private GeneratedResolver()
        {
        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            internal static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    Formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(9)
            {
                { typeof(global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>), 0 },
                { typeof(global::System.Collections.Generic.List<global::Assets.Scripts.MultiHandednessData>), 1 },
                { typeof(global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>), 2 },
                { typeof(global::Assets.Scenes.FaceTracking.FaceKeypoints), 3 },
                { typeof(global::Assets.Scenes.FaceTracking.Quat), 4 },
                { typeof(global::Assets.Scenes.FaceTracking.Vec3), 5 },
                { typeof(global::Assets.Scripts.BodyData), 6 },
                { typeof(global::Assets.Scripts.HandsMediapipeData), 7 },
                { typeof(global::Assets.Scripts.MultiHandednessData), 8 },
            };
        }

        internal static object GetFormatter(global::System.Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ListFormatter<global::Assets.Scenes.FaceTracking.Vec3>();
                case 1: return new global::MessagePack.Formatters.ListFormatter<global::Assets.Scripts.MultiHandednessData>();
                case 2: return new global::MessagePack.Formatters.ListFormatter<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>();
                case 3: return new MessagePack.Formatters.Assets.Scenes.FaceTracking.FaceKeypointsFormatter();
                case 4: return new MessagePack.Formatters.Assets.Scenes.FaceTracking.QuatFormatter();
                case 5: return new MessagePack.Formatters.Assets.Scenes.FaceTracking.Vec3Formatter();
                case 6: return new MessagePack.Formatters.Assets.Scripts.BodyDataFormatter();
                case 7: return new MessagePack.Formatters.Assets.Scripts.HandsMediapipeDataFormatter();
                case 8: return new MessagePack.Formatters.Assets.Scripts.MultiHandednessDataFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1649 // File name should match first type name




// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Assets.Scenes.FaceTracking
{
    public sealed class FaceKeypointsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Assets.Scenes.FaceTracking.FaceKeypoints>
    {
        // vertices
        private static global::System.ReadOnlySpan<byte> GetSpan_vertices() => new byte[1 + 8] { 168, 118, 101, 114, 116, 105, 99, 101, 115 };
        // pos
        private static global::System.ReadOnlySpan<byte> GetSpan_pos() => new byte[1 + 3] { 163, 112, 111, 115 };
        // rot
        private static global::System.ReadOnlySpan<byte> GetSpan_rot() => new byte[1 + 3] { 163, 114, 111, 116 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Assets.Scenes.FaceTracking.FaceKeypoints value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(3);
            writer.WriteRaw(GetSpan_vertices());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>(formatterResolver).Serialize(ref writer, value.vertices, options);
            writer.WriteRaw(GetSpan_pos());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Assets.Scenes.FaceTracking.Vec3>(formatterResolver).Serialize(ref writer, value.pos, options);
            writer.WriteRaw(GetSpan_rot());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Assets.Scenes.FaceTracking.Quat>(formatterResolver).Serialize(ref writer, value.rot, options);
        }

        public global::Assets.Scenes.FaceTracking.FaceKeypoints Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var ____result = new global::Assets.Scenes.FaceTracking.FaceKeypoints();

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 8:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 8315161591585858934UL) { goto FAIL; }

                        ____result.vertices = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 3:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 7565168UL:
                                ____result.pos = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Assets.Scenes.FaceTracking.Vec3>(formatterResolver).Deserialize(ref reader, options);
                                continue;
                            case 7630706UL:
                                ____result.rot = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Assets.Scenes.FaceTracking.Quat>(formatterResolver).Deserialize(ref reader, options);
                                continue;
                        }

                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class QuatFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Assets.Scenes.FaceTracking.Quat>
    {
        // x
        private static global::System.ReadOnlySpan<byte> GetSpan_x() => new byte[1 + 1] { 161, 120 };
        // y
        private static global::System.ReadOnlySpan<byte> GetSpan_y() => new byte[1 + 1] { 161, 121 };
        // z
        private static global::System.ReadOnlySpan<byte> GetSpan_z() => new byte[1 + 1] { 161, 122 };
        // w
        private static global::System.ReadOnlySpan<byte> GetSpan_w() => new byte[1 + 1] { 161, 119 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Assets.Scenes.FaceTracking.Quat value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteMapHeader(4);
            writer.WriteRaw(GetSpan_x());
            writer.Write(value.x);
            writer.WriteRaw(GetSpan_y());
            writer.Write(value.y);
            writer.WriteRaw(GetSpan_z());
            writer.Write(value.z);
            writer.WriteRaw(GetSpan_w());
            writer.Write(value.w);
        }

        public global::Assets.Scenes.FaceTracking.Quat Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadMapHeader();
            var __x__ = default(float);
            var __y__ = default(float);
            var __z__ = default(float);
            var __w__ = default(float);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 1:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 120UL:
                                __x__ = reader.ReadSingle();
                                continue;
                            case 121UL:
                                __y__ = reader.ReadSingle();
                                continue;
                            case 122UL:
                                __z__ = reader.ReadSingle();
                                continue;
                            case 119UL:
                                __w__ = reader.ReadSingle();
                                continue;
                        }

                }
            }

            var ____result = new global::Assets.Scenes.FaceTracking.Quat(__x__, __y__, __z__, __w__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class Vec3Formatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Assets.Scenes.FaceTracking.Vec3>
    {
        // x
        private static global::System.ReadOnlySpan<byte> GetSpan_x() => new byte[1 + 1] { 161, 120 };
        // y
        private static global::System.ReadOnlySpan<byte> GetSpan_y() => new byte[1 + 1] { 161, 121 };
        // z
        private static global::System.ReadOnlySpan<byte> GetSpan_z() => new byte[1 + 1] { 161, 122 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Assets.Scenes.FaceTracking.Vec3 value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteMapHeader(3);
            writer.WriteRaw(GetSpan_x());
            writer.Write(value.x);
            writer.WriteRaw(GetSpan_y());
            writer.Write(value.y);
            writer.WriteRaw(GetSpan_z());
            writer.Write(value.z);
        }

        public global::Assets.Scenes.FaceTracking.Vec3 Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadMapHeader();
            var __x__ = default(float);
            var __y__ = default(float);
            var __z__ = default(float);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 1:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 120UL:
                                __x__ = reader.ReadSingle();
                                continue;
                            case 121UL:
                                __y__ = reader.ReadSingle();
                                continue;
                            case 122UL:
                                __z__ = reader.ReadSingle();
                                continue;
                        }

                }
            }

            var ____result = new global::Assets.Scenes.FaceTracking.Vec3(__x__, __y__, __z__);
            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Assets.Scripts
{
    public sealed class BodyDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Assets.Scripts.BodyData>
    {
        // Body
        private static global::System.ReadOnlySpan<byte> GetSpan_Body() => new byte[1 + 4] { 164, 66, 111, 100, 121 };
        // Hands
        private static global::System.ReadOnlySpan<byte> GetSpan_Hands() => new byte[1 + 5] { 165, 72, 97, 110, 100, 115 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Assets.Scripts.BodyData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(2);
            writer.WriteRaw(GetSpan_Body());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>(formatterResolver).Serialize(ref writer, value.Body, options);
            writer.WriteRaw(GetSpan_Hands());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Assets.Scripts.HandsMediapipeData>(formatterResolver).Serialize(ref writer, value.Hands, options);
        }

        public global::Assets.Scripts.BodyData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var ____result = new global::Assets.Scripts.BodyData();

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 4:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 2036625218UL) { goto FAIL; }

                        ____result.Body = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 5:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 495606194504UL) { goto FAIL; }

                        ____result.Hands = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::Assets.Scripts.HandsMediapipeData>(formatterResolver).Deserialize(ref reader, options);
                        continue;

                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class HandsMediapipeDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Assets.Scripts.HandsMediapipeData>
    {
        // Landmarks
        private static global::System.ReadOnlySpan<byte> GetSpan_Landmarks() => new byte[1 + 9] { 169, 76, 97, 110, 100, 109, 97, 114, 107, 115 };
        // MultiHandedness
        private static global::System.ReadOnlySpan<byte> GetSpan_MultiHandedness() => new byte[1 + 15] { 175, 77, 117, 108, 116, 105, 72, 97, 110, 100, 101, 100, 110, 101, 115, 115 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Assets.Scripts.HandsMediapipeData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(2);
            writer.WriteRaw(GetSpan_Landmarks());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>>(formatterResolver).Serialize(ref writer, value.Landmarks, options);
            writer.WriteRaw(GetSpan_MultiHandedness());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Assets.Scripts.MultiHandednessData>>(formatterResolver).Serialize(ref writer, value.MultiHandedness, options);
        }

        public global::Assets.Scripts.HandsMediapipeData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var ____result = new global::Assets.Scripts.HandsMediapipeData();

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 9:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_Landmarks().Slice(1))) { goto FAIL; }

                        ____result.Landmarks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::System.Collections.Generic.List<global::Assets.Scenes.FaceTracking.Vec3>>>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 15:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_MultiHandedness().Slice(1))) { goto FAIL; }

                        ____result.MultiHandedness = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Assets.Scripts.MultiHandednessData>>(formatterResolver).Deserialize(ref reader, options);
                        continue;

                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class MultiHandednessDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Assets.Scripts.MultiHandednessData>
    {
        // index
        private static global::System.ReadOnlySpan<byte> GetSpan_index() => new byte[1 + 5] { 165, 105, 110, 100, 101, 120 };
        // label
        private static global::System.ReadOnlySpan<byte> GetSpan_label() => new byte[1 + 5] { 165, 108, 97, 98, 101, 108 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Assets.Scripts.MultiHandednessData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(2);
            writer.WriteRaw(GetSpan_index());
            writer.Write(value.index);
            writer.WriteRaw(GetSpan_label());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.label, options);
        }

        public global::Assets.Scripts.MultiHandednessData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var ____result = new global::Assets.Scripts.MultiHandednessData();

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 5:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 517097156201UL:
                                ____result.index = reader.ReadInt32();
                                continue;
                            case 465557414252UL:
                                ____result.label = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                                continue;
                        }

                }
            }

            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

