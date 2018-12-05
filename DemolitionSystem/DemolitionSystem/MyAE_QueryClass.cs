using System;
using System.Collections.Generic;
using System.Text;

//using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
//using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;

namespace B3CSecoGIS
{
    class queryclass
    {
        //根据图层名找到图层对象
        private ILayer GetLayerFromName(IMap pmap, string layername)
        {
            ILayer plyr = null;

            if (pmap.LayerCount == 0) return null;
            for (int i = 0; i < pmap.LayerCount; i++)
            {
                if (pmap.get_Layer(i).Name == layername)
                {
                    plyr = pmap.get_Layer(i);
                    break;
                }

            }

            return plyr;

        }

        public IFeatureCursor query_fsdlayer(IMap mpmap, IGeometry sgeomerty, string layername, esriSpatialRelEnum espl)
        {

            IFeatureCursor featurecursor = null;

            if (mpmap.LayerCount != 0)
            {
                ILayer lyr = GetLayerFromName(mpmap, layername);

                IFeatureLayer featurelyr = lyr as IFeatureLayer;
                IFeatureClass featurecls = featurelyr.FeatureClass;

                ISpatialFilter pspatialfl = new SpatialFilterClass();
                IQueryFilter pqueryfl = pspatialfl as ISpatialFilter;
                pspatialfl.Geometry = sgeomerty;

                pspatialfl.SpatialRel = espl;

                featurecursor = featurelyr.Search(pqueryfl, true);

            }

            return featurecursor;


        }
        public void select_features_on_map(IMapControl2 mapcontrol, IGeometry sgeo, esriSelectionResultEnum esrenum)
        {
            if (mapcontrol.Map.LayerCount != 0)
            {
                ISelectionEnvironment env = new SelectionEnvironmentClass();
                env.CombinationMethod = esrenum;
                mapcontrol.Map.SelectByShape(sgeo, env, false);
            }
        }

        public void select_features_on_layer(IGeometry sgeo, IMapControl2 mapcontrol, byte layerindex, IColor scolor, esriSpatialRelEnum esrelenum)
        {
            if (mapcontrol.LayerCount != 0)
            {
                int layerindex_int = (int)layerindex;
                ILayer lyr = mapcontrol.Map.get_Layer(layerindex_int);

                IFeatureLayer featurelyr = lyr as IFeatureLayer;
                IFeatureSelection featuresel = featurelyr as IFeatureSelection;

                ISpatialFilter pspatialfl = new SpatialFilterClass();
                IQueryFilter pqueryfl = pspatialfl as ISpatialFilter;
                pspatialfl.Geometry = sgeo;
                pspatialfl.SpatialRel = esrelenum;
                featuresel.SelectFeatures(pqueryfl, esriSelectionResultEnum.esriSelectionResultNew, false);

                featuresel.SelectionColor = scolor;
            }
        }


    }
}
