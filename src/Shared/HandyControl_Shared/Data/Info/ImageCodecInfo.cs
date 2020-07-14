using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using HandyControl.Tools.Interop;

namespace HandyControl.Data
{
    internal sealed class ImageCodecInfo
    {
        private string _dllName;

        public Guid Clsid { get; set; }

        public Guid FormatID { get; set; }

        public string CodecName { get; set; }

        public string DllName
        {
            [SuppressMessage("Microsoft.Security", "CA2103:ReviewImperativeSecurity")]
            get
            {
                if (_dllName != null)
                {
                    new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.PathDiscovery, _dllName).Demand();
                }
                return _dllName;
            }
            [SuppressMessage("Microsoft.Security", "CA2103:ReviewImperativeSecurity")]
            set
            {
                if (value != null)
                {
                    new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.PathDiscovery, value).Demand();
                }
                _dllName = value;
            }
        }

        public string FormatDescription { get; set; }

        public string FilenameExtension { get; set; }

        public string MimeType { get; set; }

        public ImageCodecFlags Flags { get; set; }

        public int Version { get; set; }

        public byte[][] SignaturePatterns { get; set; }

        public byte[][] SignatureMasks { get; set; }

        public static ImageCodecInfo[] GetImageDecoders()
        {
            ImageCodecInfo[] imageCodecs;

            var status = InteropMethods.Gdip.GdipGetImageDecodersSize(out var numDecoders, out var size);

            if (status != InteropMethods.Gdip.Ok)
            {
                throw InteropMethods.Gdip.StatusException(status);
            }

            var memory = Marshal.AllocHGlobal(size);

            try
            {
                status = InteropMethods.Gdip.GdipGetImageDecoders(numDecoders, size, memory);

                if (status != InteropMethods.Gdip.Ok)
                {
                    throw InteropMethods.Gdip.StatusException(status);
                }

                imageCodecs = ConvertFromMemory(memory, numDecoders);
            }
            finally
            {
                Marshal.FreeHGlobal(memory);
            }

            return imageCodecs;
        }

        public static ImageCodecInfo[] GetImageEncoders()
        {
            ImageCodecInfo[] imageCodecs;

            var status = InteropMethods.Gdip.GdipGetImageEncodersSize(out var numEncoders, out var size);

            if (status != InteropMethods.Gdip.Ok)
            {
                throw InteropMethods.Gdip.StatusException(status);
            }

            var memory = Marshal.AllocHGlobal(size);

            try
            {
                status = InteropMethods.Gdip.GdipGetImageEncoders(numEncoders, size, memory);

                if (status != InteropMethods.Gdip.Ok)
                {
                    throw InteropMethods.Gdip.StatusException(status);
                }

                imageCodecs = ConvertFromMemory(memory, numEncoders);
            }
            finally
            {
                Marshal.FreeHGlobal(memory);
            }

            return imageCodecs;
        }

        public static ImageCodecInfo[] ConvertFromMemory(IntPtr memoryStart, int numCodecs)
        {
            var codecs = new ImageCodecInfo[numCodecs];

            int index;

            for (index = 0; index < numCodecs; index++)
            {
                var curcodec = (IntPtr)((long)memoryStart + Marshal.SizeOf(typeof(InteropValues.ImageCodecInfoPrivate)) * index);
                var codecp = new InteropValues.ImageCodecInfoPrivate();
                InteropMethods.PtrToStructure(curcodec, codecp);

                codecs[index] = new ImageCodecInfo
                {
                    Clsid = codecp.Clsid,
                    FormatID = codecp.FormatID,
                    CodecName = Marshal.PtrToStringUni(codecp.CodecName),
                    DllName = Marshal.PtrToStringUni(codecp.DllName),
                    FormatDescription = Marshal.PtrToStringUni(codecp.FormatDescription),
                    FilenameExtension = Marshal.PtrToStringUni(codecp.FilenameExtension),
                    MimeType = Marshal.PtrToStringUni(codecp.MimeType),
                    Flags = (ImageCodecFlags) codecp.Flags,
                    Version = codecp.Version,
                    SignaturePatterns = new byte[codecp.SigCount][],
                    SignatureMasks = new byte[codecp.SigCount][]
                };

                for (var j = 0; j < codecp.SigCount; j++)
                {
                    codecs[index].SignaturePatterns[j] = new byte[codecp.SigSize];
                    codecs[index].SignatureMasks[j] = new byte[codecp.SigSize];

                    Marshal.Copy((IntPtr)((long)codecp.SigMask + j * codecp.SigSize), codecs[index].SignatureMasks[j], 0, codecp.SigSize);
                    Marshal.Copy((IntPtr)((long)codecp.SigPattern + j * codecp.SigSize), codecs[index].SignaturePatterns[j], 0, codecp.SigSize);
                }
            }

            return codecs;
        }
    }
}