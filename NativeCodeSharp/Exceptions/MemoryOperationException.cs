using System;
using System.ComponentModel;
using System.Runtime.Serialization;


namespace NativeCodeSharp.Exceptions
{
    /// <summary>
    /// An exception class for memory operation.
    /// </summary>
    [Serializable]
    public class MemoryOperationException : Win32Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.Exception class.
        /// </summary>
        public MemoryOperationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> with a specified error code.
        /// </summary>
        /// <param name="error">An error code of Win32 API,</param>
        public MemoryOperationException(int error)
            : base(error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MemoryOperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> with a specified error code and message.
        /// </summary>
        /// <param name="error">An error code of Win32 API,</param>
        /// <param name="message">The message that describes the error.</param>
        public MemoryOperationException(int error, string message)
            : base(error, message)
        {
        }

        /// <summary>
        /// Create instance with a given message and an exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MemoryOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
        protected MemoryOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
