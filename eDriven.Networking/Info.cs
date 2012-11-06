namespace eDriven.Networking
{
    public class Info
    {
        public const string AssemblyName = "eDriven.Networking";
        public const string AssemblyVersion = "1.0.5";
        public const string Author = "Danko Kozar";
        public const string Copyright = "2012 by Danko Kozar";
        public const string Web = "edriven.dankokozar.com";

        public override string ToString()
        {
            return string.Format(@"[{0} {1}]
[by {2}, (C) {3}, {4}]", AssemblyName, AssemblyVersion, Author, Copyright, Web);
        }
    }
}