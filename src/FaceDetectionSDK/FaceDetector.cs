using System.Runtime.InteropServices;

namespace FaceDetectionSDK;

public static partial class FaceDetector
{

    private const string MxFRSDK_PATH = "MxFRSDK_DemoApp";

    [LibraryImport(MxFRSDK_PATH, EntryPoint = "release_library_instance")]
    public static partial int releaseLibraryInstance(in ulong libraryInstance);

    [LibraryImport(MxFRSDK_PATH, EntryPoint = "release_sdk_instance")]
    public static partial int releaseSdkInstance(in ulong sdkInstance);

    [LibraryImport(MxFRSDK_PATH, EntryPoint = "initialize_library_instance", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    public static partial int initializeLibraryInstance(out ulong libraryInstance, string modelDir);

    [LibraryImport(MxFRSDK_PATH, EntryPoint = "initialize_sdk_instance", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    public static partial int initializeSdkInstance(string imgPath, int benchmarkSelection, in ulong libraryInstance, out ulong sdkInstance);

    [LibraryImport(MxFRSDK_PATH, EntryPoint = "detect_face", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    public static partial int detectFace(out float mFaceXC, out float mFaceYC, out float mFaceHeight, out float mFaceWidth, out float mFaceScore,
        string imgPath, int benchmark, in ulong sdkInstance); 

}
