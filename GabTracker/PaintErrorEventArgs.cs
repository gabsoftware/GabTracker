using System;

namespace GabTracker
{
    /// <summary>
    /// Provides data for the GabTracker.PaintError event.
    /// </summary>
    public class PaintErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The exception that occurred during painting.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of PaintErrorEventArgs.
        /// </summary>
        public PaintErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
