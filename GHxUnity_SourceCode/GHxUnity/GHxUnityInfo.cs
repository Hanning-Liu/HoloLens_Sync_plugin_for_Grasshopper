using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GHxUnity
{
    public class GHxUnityInfo : GH_AssemblyInfo
    {
        public override string Name => "GHxUnity";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("eb829662-d500-41fd-bd64-7cc5bcd266ad");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}