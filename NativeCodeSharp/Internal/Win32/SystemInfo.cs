using System;
using System.Runtime.InteropServices;


namespace NativeCodeSharp.Internal.Win32
{
    /// <summary>
    /// <para>Contains information about the current computer system.</para>
    /// <para>This includes the architecture and type of the processor, the number of processors in the system, the page size, and other such information.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    internal struct SystemInfo
    {
        /// <summary>
        /// The processor architecture of the installed operating system.
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture { get; private set; }
        /// <summary>
        /// This member is reserved for future use.
        /// </summary>
        private ushort Reserved { get; set; }
        /// <summary>
        /// <para>The page size and the granularity of page protection and commitment.</para>
        /// <para>This is the page size used by the VirtualAlloc function.</para>
        /// </summary>
        public uint PageSize { get; private set; }
        /// <summary>
        /// A pointer to the lowest memory address accessible to applications and dynamic-link libraries (DLLs).
        /// </summary>
        public IntPtr MinimumApplicationAddress { get; private set; }
        /// <summary>
        /// A pointer to the highest memory address accessible to applications and DLLs.
        /// </summary>
        public IntPtr MaximumApplicationAddress { get; private set; }
        /// <summary>
        /// <para>A mask representing the set of processors configured into the system.</para>
        /// <para>Bit 0 is processor 0; bit 31 is processor 31.</para>
        /// </summary>
        public IntPtr ActiveProcessorMask { get; private set; }
        /// <summary>
        /// <para>The number of logical processors in the current group.</para>
        /// <para>To retrieve this value, use the GetLogicalProcessorInformation function.</para>
        /// </summary>
        public uint NumberOfProcessors { get; private set; }
        /// <summary>
        /// <para>An obsolete member that is retained for compatibility.</para>
        /// <para>Use the wProcessorArchitecture, wProcessorLevel, and wProcessorRevision members to determine the type of processor.</para>
        /// </summary>
        public ProcessorType ProcessorType { get; private set; }
        /// <summary>
        /// <para>The granularity for the starting address at which virtual memory can be allocated.</para>
        /// <para>For more information, see VirtualAlloc.</para>
        /// </summary>
        public uint AllocationGranularity { get; private set; }
        /// <summary>
        /// <para>The architecture-dependent processor level.</para>
        /// <para> It should be used only for display purposes.</para>
        /// <para>To determine the feature set of a processor, use the IsProcessorFeaturePresent function.</para>
        /// </summary>
        public ushort ProcessorLevel { get; private set; }
        /// <summary>
        /// <para>The architecture-dependent processor revision.</para>
        /// <para>The following table shows how the revision value is assembled for each type of processor architecture.</para>
        /// </summary>
        public ushort ProcessorRevision { get; private set; }
    }
}
