using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GHxUnity
{
    public class Unity2GH_Eye_Component : GH_Component
    {
        Point3d temp_p;
        Vector3d temp_vForward;
        public Unity2GH_Eye_Component()
          : base("Unity2GH_Eye", "U2G_Eye",
              "Receive Eye Info from Unity (need \"Trigger\" Component",
              BasicInfo.Category, BasicInfo.Sub_Category_U2G)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Eye_Pos", "P", "The position of the Eye in Unity coordinate system.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Eye_Dir", "Dir", "The direction of the Eye in Unity coordinate system.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Register(this);
            DA.SetData(0, temp_p);
            DA.SetData(1, temp_vForward);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource.U2G_Eye;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("6AD446EE-5199-4EA7-A2E5-02B5C6FBDBD6"); }
        }
        void Register(IGH_Component component)
        {
            Rhino.Runtime.HostUtils.RegisterNamedCallback("ToGH_EyeGaze", ToGrasshopper);
        }
        void ToGrasshopper(object sender, Rhino.Runtime.NamedParametersEventArgs args)
        {
            Point3d p;
            Vector3d vForward;

            if (args.TryGetPoint("HLEyePos", out p))
            {
                args.TryGetVector("HLEyeForward", out vForward);

                temp_p = p;
                temp_vForward = vForward;
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}