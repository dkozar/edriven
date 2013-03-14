using System.Text;

namespace eDriven.Core.Signals
{
    public static class SignalUtil
    {
        private static StringBuilder _sb;
        private static string _s;

        public static string DescribeParams(params object[] parameters)
        {
            if (null == _sb)
                _sb = new StringBuilder();

            var count = parameters.Length;
            for (int i = 0; i < count; i++)
            {
                _sb.AppendLine(string.Format("    [{0}] {1} [{2}]", i, parameters[i], parameters[i].GetType()));
            }

            _s = _sb.ToString();
            _sb.Remove(0, _sb.Length);
            return _s;
        }
    }
}
