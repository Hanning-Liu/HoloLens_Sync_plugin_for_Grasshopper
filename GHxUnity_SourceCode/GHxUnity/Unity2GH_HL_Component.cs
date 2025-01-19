using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GHxUnity
{
    public class Unity2GH_HL_Component : GH_Component
    {
        Point3d temp_p;
        Vector3d temp_vRight;
        Vector3d temp_vUp;
        public Unity2GH_HL_Component()
          : base("Unity2GH_HL", "U2G_HL",
              "Receive HL Info from Unity (need \"Trigger\" Component)",
              BasicInfo.Category, BasicInfo.Sub_Category_U2G)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("HL_Plane", "P", "The plane of QRCode in Unity coordinate system.", GH_ParamAccess.item);
            //pManager.AddPointParameter("HL_Pos", "P", "The top left corner of the HL in Unity coordinate system.", GH_ParamAccess.item);
            //pManager.AddVectorParameter("HL_X", "X", "The right direction (+X) of the HL in Unity coordinate system.", GH_ParamAccess.item);
            //pManager.AddVectorParameter("HL_Y", "Y", "The top direction (+Y) of the HL in Unity coordinate system.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Register(this);
            Plane p = new Plane(temp_p, temp_vRight, temp_vUp);
            DA.SetData(0, p);
            //DA.SetData(0, temp_p);
            //DA.SetData(1, temp_vRight);
            //DA.SetData(2, temp_vUp);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource.U2G_Head;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("67F2C1B4-4161-42B2-B246-22C76CE12FB8"); }
        }

        void Register(IGH_Component component)
        {
            Rhino.Runtime.HostUtils.RegisterNamedCallback("ToGH_CamPose", ToGrasshopper);
        }

        void ToGrasshopper(object sender, Rhino.Runtime.NamedParametersEventArgs args)
        {
            Point3d p;
            Vector3d vRight;
            Vector3d vUp;
            if (args.TryGetPoint("CamPos", out p))
            {
                args.TryGetVector("CamvRight", out vRight);
                args.TryGetVector("CamvUp", out vUp);
                temp_p = p;
                temp_vRight = vRight;
                temp_vUp = vUp;
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}