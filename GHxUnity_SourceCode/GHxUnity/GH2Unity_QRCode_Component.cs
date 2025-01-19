using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types.Transforms;
using Rhino.Geometry;
using System.Numerics;


namespace GHxUnity
{
    public class GH2Unity_QRCode_Component : GH_Component
    {

        public GH2Unity_QRCode_Component()
          : base("GH2Unity_QRCode", "G2U_QRCode",
              "Send QRCode Info from GH to Unity",
              BasicInfo.Category, BasicInfo.Sub_Category_G2U)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Position", "P", "The top left corner of the QRCode.", GH_ParamAccess.item);
            pManager.AddVectorParameter("X-Axis", "X", "The right direction (+X) of the QRCode.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Y-Axis", "Y", "The top direction (+Y) of the QRCode.", GH_ParamAccess.item);
            pManager.AddTextParameter("Content", "T", "The content stored in the QRCode.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d origin = Point3d.Unset;
            Vector3d x = Vector3d.Unset;
            Vector3d y = Vector3d.Unset;
            String content_str = null;
            if(!DA.GetData(0, ref origin) || !DA.GetData(1, ref x) || !DA.GetData(2, ref y) || !DA.GetData(3, ref content_str))
                return;

            Vector3d z = Vector3d.CrossProduct(x, y);

            Matrix4x4 T = Matrix4x4.Identity;
            T.M11 = (float)x.X; T.M12 = (float)y.X; T.M13 = (float)z.X;
            T.M21 = (float)x.Y; T.M22 = (float)y.Y; T.M23 = (float)z.Y;
            T.M31 = (float)x.Z; T.M32 = (float)y.Z; T.M33 = (float)z.Z;
            
            System.Numerics.Quaternion rotation = System.Numerics.Quaternion.CreateFromRotationMatrix(T);
            String position_str =  origin.ToString();
            String rotation_str = rotation.ToString();

            using (var args = new Rhino.Runtime.NamedParametersEventArgs())
            {
                args.Set("position", position_str);
                args.Set("rotation", rotation_str);
                args.Set("content", content_str);

                Rhino.Runtime.HostUtils.ExecuteNamedCallback("FromGHMarkerList", args);
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                
                return Resource.G2U_QRCode;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0E5C2D6C-C23E-4C89-A18D-5B8D1C884958"); }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
    }
}