using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GHxUnity
{
    public class Unity2GH_Hand_Component : GH_Component
    {
        string[] temp_pos_L;
        string[] temp_Up_L;
        string[] temp_Right_L;
        string[] temp_pos_R;
        string[] temp_Up_R;
        string[] temp_Right_R;
        public Unity2GH_Hand_Component()
          : base("Unity2GH_Hand", "U2G_Hand",
              "Receive Hand Joints Info from Unity (need \"Trigger\" Component",
              BasicInfo.Category, BasicInfo.Sub_Category_U2G)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Left_Hand_Pos", "L_P", "The position of left hand joints.", GH_ParamAccess.list);
            pManager.AddTextParameter("Left_Hand_Up", "L_Up", "The Up_Dir of left hand joints.", GH_ParamAccess.list);
            pManager.AddTextParameter("Left_Hand_Right", "L_Right", "The Right_Dir of left hand joints.", GH_ParamAccess.list);
            pManager.AddTextParameter("Right_Hand_Pos", "R_P", "The position of right hand joints.", GH_ParamAccess.list);
            pManager.AddTextParameter("Right_Hand_Up", "R_Up", "The Up_Dir of right hand joints.", GH_ParamAccess.list);
            pManager.AddTextParameter("Right_Hand_Right", "R_Right", "The Right_Dir of right hand joints.", GH_ParamAccess.list);


        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Register(this);
            DA.SetDataList(0, temp_pos_L);
            DA.SetDataList(1, temp_Up_L);
            DA.SetDataList(2, temp_Right_L);
            DA.SetDataList(3, temp_pos_R);
            DA.SetDataList(4, temp_Up_R);
            DA.SetDataList(5, temp_Right_R);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resource.U2G_Hand;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("4379607D-7C58-4B6B-B226-C0CED90320D8"); }
        }
        void Register(IGH_Component component)
        {
            Rhino.Runtime.HostUtils.RegisterNamedCallback("ToGH_HandJoints", ToGrasshopper);
        }

        void ToGrasshopper(object sender, Rhino.Runtime.NamedParametersEventArgs args)
        {
            string[] pos_L;
            string[] Up_L;
            string[] Right_L;
            string[] pos_R;
            string[] Up_R;
            string[] Right_R;

            if (args.TryGetStrings("pos_L", out pos_L))
            {
                args.TryGetStrings("Up_L", out Up_L);
                args.TryGetStrings("Right_L", out Right_L);
                args.TryGetStrings("pos_R", out pos_R);
                args.TryGetStrings("Up_R", out Up_R);
                args.TryGetStrings("Right_R", out Right_R);

                temp_pos_L = pos_L;
                temp_Up_L = Up_L;
                temp_Right_L = Right_L;
                temp_pos_R = pos_R;
                temp_Up_R = Up_R;
                temp_Right_R = Right_R;

            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}