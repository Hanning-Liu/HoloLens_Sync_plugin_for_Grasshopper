using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GHxUnity
{
    public class Unity2GH_QRCode_Component : GH_Component
    {
        Point3d temp_p;
        Vector3d temp_vRight;
        Vector3d temp_vUp;
        public Unity2GH_QRCode_Component()
          : base("Unity2GH_QRCode", "U2G_QRCode",
              "Receive QRCode Info from Unity (need \"Trigger\" Component)",
              BasicInfo.Category, BasicInfo.Sub_Category_U2G)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("QRCode_Plane", "P", "The plane of QRCode in Unity coordinate system.", GH_ParamAccess.item);
            //pManager.AddPointParameter("QRCode_Pos", "P", "The top left corner of the QRCode in Unity coordinate system.", GH_ParamAccess.item);
            //pManager.AddVectorParameter("QRCode_X", "X", "The right direction (+X) of the QRCode in Unity coordinate system.", GH_ParamAccess.item);
            //pManager.AddVectorParameter("QRCode_Y", "Y", "The top direction (+Y) of the QRCode in Unity coordinate system.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Register(this);
            Plane p = new Plane(temp_p, temp_vRight, temp_vUp);
            DA.SetData(0, p);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource.U2G_QRCode;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0E4508CB-6F4C-4170-BE83-392A5EF235B9"); }
        }
        void Register(IGH_Component component)
        {
            Rhino.Runtime.HostUtils.RegisterNamedCallback("ToGH_MarkerPose", ToGrasshopper);
        }
        void ToGrasshopper(object sender, Rhino.Runtime.NamedParametersEventArgs args)
        {
            Point3d p;
            Vector3d vRight;
            Vector3d vUp;
            if (args.TryGetPoint("MarkerPos", out p))
            {
                args.TryGetVector("MarkervRight", out vRight);
                args.TryGetVector("MarkervUp", out vUp);
                temp_p = p;
                temp_vRight = vRight;
                temp_vUp = vUp;
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
    }
}