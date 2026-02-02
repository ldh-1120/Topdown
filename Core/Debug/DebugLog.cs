using System.Diagnostics;

namespace Basement.Core.Debug {
    public class DebugLog {
        [Conditional("DEBUG")]
        public static void Log(string msg) => System.Diagnostics.Debug.WriteLine(msg);
        
        [Conditional("DEBUG")]
        public static void Warn(string msg) => System.Diagnostics.Debug.WriteLine($"[WARN]  {msg}");
        
        [Conditional("DEBUG")]
        public static void Error(string msg) => System.Diagnostics.Debug.WriteLine($"[ERROR]  {msg}");
    }
}