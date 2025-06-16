using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents standard Unix signal numbers, grouped by category.
    /// </summary>
    public enum UnixSignal
    {
        // ──────────────── Termination & Interruption ────────────────

        /// <summary>Hang up detected on controlling terminal or
        /// death of controlling process (SIGHUP).</summary>
        SIGHUP = 1,

        /// <summary>Interrupt from keyboard (SIGINT).</summary>
        SIGINT = 2,

        /// <summary>Quit from keyboard (SIGQUIT).</summary>
        SIGQUIT = 3,

        /// <summary>Termination signal (SIGTERM).</summary>
        SIGTERM = 15,

        /// <summary>Abort signal from abort() (SIGABRT).</summary>
        SIGABRT = 6,

        /// <summary>Kills the process immediately (SIGKILL). Cannot be caught or ignored.</summary>
        SIGKILL = 9,

        /// <summary>Software termination signal (SIGXCPU - CPU time limit exceeded).</summary>
        SIGXCPU = 24,

        /// <summary>File size limit exceeded (SIGXFSZ).</summary>
        SIGXFSZ = 25,

        /// <summary>Virtual alarm clock (SIGVTALRM).</summary>
        SIGVTALRM = 26,

        /// <summary>Profiling timer expired (SIGPROF).</summary>
        SIGPROF = 27,

        // ──────────────── Errors & Exceptions ────────────────

        /// <summary>Illegal instruction (SIGILL).</summary>
        SIGILL = 4,

        /// <summary>Floating-point exception (SIGFPE).</summary>
        SIGFPE = 8,

        /// <summary>Invalid memory reference (SIGSEGV).</summary>
        SIGSEGV = 11,

        /// <summary>Bus error (SIGBUS).</summary>
        SIGBUS = 7,

        /// <summary>Broken pipe (SIGPIPE).</summary>
        SIGPIPE = 13,

        // ──────────────── Debugging & Traps ────────────────

        /// <summary>Trace/breakpoint trap (SIGTRAP).</summary>
        SIGTRAP = 5,

        /// <summary>Child stopped or terminated (SIGCHLD).</summary>
        SIGCHLD = 17,

        // ──────────────── Job & Terminal Control ────────────────

        /// <summary>Stop process (SIGSTOP). Cannot be caught or ignored.</summary>
        SIGSTOP = 19,

        /// <summary>Terminal stop signal (SIGTSTP). Typically sent by Ctrl+Z.</summary>
        SIGTSTP = 20,

        /// <summary>Continue executing, if stopped (SIGCONT).</summary>
        SIGCONT = 18,

        /// <summary>Background process read from terminal (SIGTTIN).</summary>
        SIGTTIN = 21,

        /// <summary>Background process write to terminal (SIGTTOU).</summary>
        SIGTTOU = 22,

        // ──────────────── Timers ────────────────

        /// <summary>Alarm clock (SIGALRM).</summary>
        SIGALRM = 14,

        /// <summary>Real-time timer expired (SIGRTMIN and up are real-time).</summary>
        SIGRTMIN = 34,

        // ──────────────── User-Defined ────────────────

        /// <summary>User-defined signal 1 (SIGUSR1).</summary>
        SIGUSR1 = 10,

        /// <summary>User-defined signal 2 (SIGUSR2).</summary>
        SIGUSR2 = 12,

        // ──────────────── System & Misc ────────────────

        /// <summary>Power failure (SIGPWR).</summary>
        SIGPWR = 30,

        /// <summary>Window resize (SIGWINCH).</summary>
        SIGWINCH = 28,
    }
}
