using Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;

namespace GHxUnity
{
    public class GH2Unity_Mesh_Component : GH_Component
    {
        IGH_Component comp = null;
        GH_Structure<GH_Mesh> prevMeshes = new GH_Structure<GH_Mesh>();
        List<int> prevDataTreeCounts = new List<int>();

        public GH2Unity_Mesh_Component()
          : base("GH2Unity_Mesh", "G2U_Mesh",
            "Send Mesh geometries from GH to Unity",
            BasicInfo.Category, BasicInfo.Sub_Category_G2U)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("meshes", "geo", "The meshes to be sent.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("mats", "mat", "The material of each geo.", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("outmeshes", "outGeo", "The Mesh to be sent.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("outmats", "outMat", "The Mat to be sent.", GH_ParamAccess.tree);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DA.GetDataTree(0, out GH_Structure<GH_Mesh> meshes) || !DA.GetDataTree(1, out GH_Structure<IGH_Goo> mats))
                return;
            meshes.Simplify(GH_SimplificationMode.CollapseAllOverlaps);
            mats.Simplify(GH_SimplificationMode.CollapseAllOverlaps);

            if (comp == null)
            {
                this.PingDocument -= OnPingDocument;
                this.PingDocument += OnPingDocument;
                this.ObjectChanged -= OnObjectChanged;
                this.ObjectChanged += OnObjectChanged;
                Instances.ActiveCanvas.DocumentChanged -= OnDocumentChanged;
                Instances.ActiveCanvas.DocumentChanged += OnDocumentChanged;

                comp = this;
            }
            

            prevMeshes = meshes;

            GH_Structure<GH_Material> ghMats = new GH_Structure<GH_Material>();
            ClearAllMeshes(meshes);

            List<int> tempDataTreeCounts = new List<int>();
            for (int i = 0; i < meshes.PathCount; i++)
            {
                GH_Path path = meshes.Paths[i];
                tempDataTreeCounts.Add(meshes.Branches[i].Count);

                List<IGH_Goo> matList = mats.Branches[Math.Min(i, mats.PathCount - 1)];
                for (int n = 0; n < meshes.Branches[i].Count; n++)
                {
                    GH_Mesh mesh = meshes.Branches[i][n];
                    object mat = matList[Math.Min(n, matList.Count - 1)];
                    using (var args = new Rhino.Runtime.NamedParametersEventArgs())
                    {
                        Color color = Color.White;
                        Color emission = Color.Black;
                        Color specular = Color.DarkGray;
                        double transparency = 0f;
                        double shine = 50;
                        if (mat != null && mat.GetType() == typeof(GH_Material))
                        {
                            GH_Material gH_Material = (GH_Material)mat;
                            var material = gH_Material.Value;
                            color = material.Diffuse;
                            emission = material.Emission;
                            transparency = material.Transparency;
                            shine = material.Shine;
                            specular = material.Specular;
                        }
                        else if (mat != null && mat.GetType() == typeof(GH_Colour))
                        {
                            GH_Colour gH_Colour = (GH_Colour)mat;
                            color = gH_Colour.Value;
                            shine = 0.5;
                        }

                        args.Set("id", this.InstanceGuid.ToString() + "-" + i.ToString() + "_" + n.ToString());
                        args.Set("mesh", new Mesh[] { mesh.Value });
                        args.Set("diffuse", color);
                        args.Set("emission", emission);
                        args.Set("specular", specular);
                        args.Set("transparency", transparency);
                        args.Set("shine", shine);
                        Rhino.Runtime.HostUtils.ExecuteNamedCallback("FromGHMesh", args);

                        var displayMat = new Rhino.Display.DisplayMaterial(color);
                        displayMat.Specular = specular;
                        displayMat.Emission = emission;
                        displayMat.Transparency = transparency;
                        displayMat.Shine = shine;

                        var ghmat = new GH_Material(displayMat);
                        ghMats.Append(ghmat, path);
                    }
                }
            }

            prevDataTreeCounts = tempDataTreeCounts;

            DA.SetDataTree(0, meshes);
            DA.SetDataTree(1, ghMats);
        }

        protected override System.Drawing.Bitmap Icon => Resource.G2U_Mesh;

        public override Guid ComponentGuid => new Guid("fa246107-1a84-488a-84c2-cb4dd627aac3");

        public void OnPingDocument(object sender, GH_PingDocumentEventArgs e)
        {
            if (e.Document == null)
            {
                ClearAllMeshes(prevMeshes);
            }
        }

        public void OnObjectChanged(object sender, GH_ObjectChangedEventArgs e)
        {
            if (e.Type == GH_ObjectEventType.Enabled)
            {
                if (comp.Locked)
                {
                    ClearAllMeshes(prevMeshes);
                }
            }

            comp.ExpireSolution(true);
        }

        public void OnDocumentChanged(object sender, GH_CanvasDocumentChangedEventArgs e)
        {
            ClearAllMeshes(prevMeshes);
            if (comp != null)
            {
                comp.ExpireSolution(true);
            }
        }

        public void ClearAllMeshes(GH_Structure<GH_Mesh> meshes)
        {
            int branchCount = Math.Max(meshes.PathCount, prevDataTreeCounts.Count);
            for (int i = 0; i < branchCount; i++)
            {
                int meshCount = 0;
                int prevCount = 0;
                if (meshes.PathCount > i)
                {
                    meshCount = meshes.Branches[i].Count;
                }
                if (prevDataTreeCounts.Count > i)
                {
                    prevCount = prevDataTreeCounts[i];
                }
                int itemCount = Math.Max(meshCount, prevCount);


                for (int n = 0; n < itemCount; n++)
                {
                    using (var args = new Rhino.Runtime.NamedParametersEventArgs())
                    {
                        args.Set("id", comp.InstanceGuid.ToString() + "-" + i.ToString() + "_" + n.ToString());
                        Rhino.Runtime.HostUtils.ExecuteNamedCallback("FromGHClearMesh", args);
                    }
                }
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}