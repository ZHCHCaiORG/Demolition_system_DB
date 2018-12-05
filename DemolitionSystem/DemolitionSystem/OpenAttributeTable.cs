
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
namespace ZhangChuanCaiNameSpace
{
    class B3CSecoGIS_OpenAttributeTable
    {
        public DataTable B3CSecoGISattributetable;
        //绑定DataTable到ＤataGridView.其中使用到其他方法，如创建空表头方法，类型转换方法。。

        public void B3CSecoGIScreateattributetable(ILayer player, DataGridView pdatagridview)
        {
            string tablename;
            tablename = getvalidfeatureclassname(player.Name);
            B3CSecoGISattributetable = B3CSecoGIScreatedatatable(player, tablename);
            pdatagridview.DataSource = B3CSecoGISattributetable;
        }
        ///   //方法重载
        ///   

        public void B3CSecoGIScreateattributetable(ILayer player, DataGridView pdatagridview, ICursor zcccursor)
        {
            string tablename;
            tablename = getvalidfeatureclassname(player.Name);
            B3CSecoGISattributetable = createdatatable(player, tablename, zcccursor);
            pdatagridview.DataSource = B3CSecoGISattributetable;
        }

        //public DataTable attributetable;
        //绑定DataTable到ＤataGridView.其中使用到其他方法，如创建空表头方法，类型转换方法。。
        public void B3CSecoGIScreateattributetable(ILayer player, DataGridView pdatagridview, IQueryFilter attritablequeryfilter)
        {
            string tablename;
            tablename = getvalidfeatureclassname(player.Name + "linshi");
            B3CSecoGISattributetable = createdatatable(player, tablename, attritablequeryfilter);
            pdatagridview.DataSource = B3CSecoGISattributetable;
        }
        //创建空表头方法。
        private static DataTable B3CSecoGIScreatedatatablebylayer(ILayer player, string tablename)
        {
            DataTable pdatatable = new DataTable(tablename);
            ITable ptable = player as ITable;
            IField pfield = null;
            DataColumn pdatacolumn;
            //根据每个字段的属性建立ＤataColumn对象
            #region ／／ＦＯＲ循环,创建表的列头。
            for (int i = 0; i < ptable.Fields.FieldCount; i++)
            {
                pfield = ptable.Fields.get_Field(i);
                pdatacolumn = new DataColumn(pfield.Name);
                if (pfield.Name == ptable.OIDFieldName)
                {
                    pdatacolumn.Unique = true;//字段是否唯一。
                }
                pdatacolumn.AllowDBNull = pfield.IsNullable;
                pdatacolumn.Caption = pfield.AliasName;
                pdatacolumn.DataType = System.Type.GetType(B3CSecoGISparsefieldtype(pfield.Type));//需要转换ＡＥ与。ＮＥＴ类型
                pdatacolumn.DefaultValue = pfield.DefaultValue;
                if (pfield.VarType == 8)
                {
                    pdatacolumn.MaxLength = pfield.Length;
                }

                //将字段添加到表中。
                pdatatable.Columns.Add(pdatacolumn);
                pfield = null;
                pdatacolumn = null;
            }
            #endregion
            return pdatatable;
        }

        //创建空表头方法。
        private static DataTable B3CSecoGIScreatedatatablebyAEtable(ITable ptable, string tablename)
        {
            DataTable pdatatable = new DataTable(tablename);
            //ITable ptable = player as ITable;
            IField pfield = null;
            DataColumn pdatacolumn;
            //根据每个字段的属性建立ＤataColumn对象
            #region ／／ＦＯＲ循环,创建表的列头。
            for (int i = 0; i < ptable.Fields.FieldCount; i++)
            {
                pfield = ptable.Fields.get_Field(i);
                pdatacolumn = new DataColumn(pfield.Name);
                if (pfield.Name == ptable.OIDFieldName)
                {
                    pdatacolumn.Unique = true;//字段是否唯一。
                }
                pdatacolumn.AllowDBNull = pfield.IsNullable;
                pdatacolumn.Caption = pfield.AliasName;
                pdatacolumn.DataType = System.Type.GetType(B3CSecoGISparsefieldtype(pfield.Type));//需要转换ＡＥ与。ＮＥＴ类型
                pdatacolumn.DefaultValue = pfield.DefaultValue;
                if (pfield.VarType == 8)
                {
                    pdatacolumn.MaxLength = pfield.Length;
                }

                //将字段添加到表中。
                pdatatable.Columns.Add(pdatacolumn);
                pfield = null;
                pdatacolumn = null;
            }
            #endregion
            return pdatatable;
        }

        //ＡＥ中的类型与．ＮＥＴ中的类型转换方法
        public static string B3CSecoGISparsefieldtype(esriFieldType fieldtype)
        {
            switch (fieldtype)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }

        }

        //获得图层的类型
        public static string B3CSecoGISgetshapetype(ILayer player)
        {

            if (player is IFeatureLayer)
            {
                IFeatureLayer pfeatlyr = player as IFeatureLayer;
                switch (pfeatlyr.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        return "Point";
                    case esriGeometryType.esriGeometryPolyline:
                        return "Polyline";
                    case esriGeometryType.esriGeometryPolygon:
                        return "Polygon";
                    default:
                        return "";
                }
            }
            else if (player is IRasterLayer)
            {
                return "raster";
            }
            return "";

        }
        //装载DATATABLE
        public static DataTable B3CSecoGIScreatedatatable(ILayer player, string tablename)
        {
            //创建空的datatable
            DataTable pdatatable = B3CSecoGIScreatedatatablebylayer(player, tablename);
            //取得图层类型
            string shapetype = B3CSecoGISgetshapetype(player);
            //创建ＤＡＴＡＴＡＢＬＥ的行对象
            DataRow pdatarow = null;
            //从ＩＬＡＹＥＲ查询到ＩＴＡＢＬＥ
            ITable ptable = player as ITable;
            ICursor pcursor = ptable.Search(null, false);
            //取得ＩＴＡＢＬＥ中的行信息
            IRow prow = pcursor.NextRow();
            int n = 0;
            while (prow != null)
            {
                //新建ＤＡＴＡＴＡＢＬＥ的行对象
                pdatarow = pdatatable.NewRow();
                for (int i = 0; i < prow.Fields.FieldCount; i++)
                {
                    if (prow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pdatarow[i] = shapetype;

                    }
                    else if (prow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                    {
                        pdatarow[i] = "Element";
                    }
                    else
                    {
                        pdatarow[i] = prow.get_Value(i);
                    }

                }

                //添加ＤＡＴＡＲＯＷ到ＤＡＴＡＴＡＢＬＥ
                pdatatable.Rows.Add(pdatarow);
                pdatarow = null;
                n++;
                if (n == 2000)
                {
                    prow = null;
                }
                else
                {
                    prow = pcursor.NextRow();
                }
            }
            return pdatatable;
        }

        //装载DATATABLE
        ////方法重载
        public static DataTable createdatatable(ILayer player, string tablename, ICursor zcccursor)
        {
            //创建空的datatable
            DataTable pdatatable = B3CSecoGIScreatedatatablebylayer(player, tablename);
            //取得图层类型
            string shapetype = B3CSecoGISgetshapetype(player);
            //创建ＤＡＴＡＴＡＢＬＥ的行对象
            DataRow pdatarow = null;
            //从ＩＬＡＹＥＲ查询到ＩＴＡＢＬＥ
            ITable ptable = player as ITable;
            ICursor pcursor = zcccursor;
            //取得ＩＴＡＢＬＥ中的行信息
            IRow prow = pcursor.NextRow();
            int n = 0;
            while (prow != null)
            {
                //新建ＤＡＴＡＴＡＢＬＥ的行对象
                pdatarow = pdatatable.NewRow();
                for (int i = 0; i < prow.Fields.FieldCount; i++)
                {
                    if (prow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pdatarow[i] = shapetype;

                    }
                    else if (prow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                    {
                        pdatarow[i] = "Element";
                    }
                    else
                    {
                        pdatarow[i] = prow.get_Value(i);
                    }

                }

                //添加ＤＡＴＡＲＯＷ到ＤＡＴＡＴＡＢＬＥ
                pdatatable.Rows.Add(pdatarow);
                pdatarow = null;
                n++;
                if (n == 2000)
                {
                    prow = null;
                }
                else
                {
                    prow = pcursor.NextRow();
                }
            }
            return pdatatable;
        }

        ////方法重载。

        //装载DATATABLE,选择符合条件的装载。
        public static DataTable createdatatable(ILayer player, string tablename, IQueryFilter tsaptialqueryfilter)
        {
            //创建空的datatable
            DataTable pdatatable = B3CSecoGIScreatedatatablebylayer(player, tablename);
            //取得图层类型
            string shapetype = B3CSecoGISgetshapetype(player);
            //创建ＤＡＴＡＴＡＢＬＥ的行对象
            DataRow pdatarow = null;
            //从ＩＬＡＹＥＲ查询到ＩＴＡＢＬＥ
            ITable ptable = player as ITable;
            ICursor pcursor = ptable.Search(tsaptialqueryfilter, false);
            //取得ＩＴＡＢＬＥ中的行信息
            IRow prow = pcursor.NextRow();
            int n = 0;
            while (prow != null)
            {
                //新建ＤＡＴＡＴＡＢＬＥ的行对象
                pdatarow = pdatatable.NewRow();
                for (int i = 0; i < prow.Fields.FieldCount; i++)
                {
                    if (prow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pdatarow[i] = shapetype;

                    }
                    else if (prow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                    {
                        pdatarow[i] = "Element";
                    }
                    else
                    {
                        pdatarow[i] = prow.get_Value(i);
                    }

                }

                //添加ＤＡＴＡＲＯＷ到ＤＡＴＡＴＡＢＬＥ
                pdatatable.Rows.Add(pdatarow);
                pdatarow = null;
                n++;
                if (n == 2000)
                {
                    prow = null;
                }
                else
                {
                    prow = pcursor.NextRow();
                }
            }
            return pdatatable;
        }

        //装载DATATABLE,将AE表装载到DataTable。该方法用于创建沉陷曲线的。
        public DataTable createdatatable(ITable ztable, ICursor pcursor, string tablename)
        {
            //创建表头。
            DataTable pdatatable = B3CSecoGIScreatedatatablebyAEtable(ztable, tablename);
            //创建ＤＡＴＡＴＡＢＬＥ的行对象
            DataRow pdatarow = null;
            ////从ＩＬＡＹＥＲ查询到ＩＴＡＢＬＥ
            //ITable ptable = ztable;
            //ICursor pcursor = ptable.Search(null, false);
            //取得ＩＴＡＢＬＥ中的行信息
            IRow prow = pcursor.NextRow();
            int n = 0;
            while (prow != null)
            {
                //新建ＤＡＴＡＴＡＢＬＥ的行对象
                pdatarow = pdatatable.NewRow();
                for (int i = 0; i < prow.Fields.FieldCount; i++)
                {
                    if (prow.Fields.get_Field(i).Name == "SLOPET")
                    {
                        pdatarow[i] = Convert.ToDouble(prow.get_Value(i)) * 10;
                    }
                    else if (prow.Fields.get_Field(i).Name == "CURVT")
                    {
                        pdatarow[i] = Convert.ToDouble(prow.get_Value(i)) * 1000;
                    }
                    else if (prow.Fields.get_Field(i).Name == "LEVELT")
                    {
                        pdatarow[i] = Convert.ToDouble(prow.get_Value(i)) * 10;
                    }
                    else if (prow.Fields.get_Field(i).Name == "SINK")
                    {
                        pdatarow[i] = Convert.ToDouble(prow.get_Value(i)) / 10;
                    }
                    else if (prow.Fields.get_Field(i).Name == "LEVELMOVE")
                    {
                        pdatarow[i] = Convert.ToDouble(prow.get_Value(i)) / 10;
                    }
                    else
                    {
                        pdatarow[i] = prow.get_Value(i);
                    }
                }

                //添加ＤＡＴＡＲＯＷ到ＤＡＴＡＴＡＢＬＥ
                pdatatable.Rows.Add(pdatarow);
                pdatarow = null;
                n++;
                if (n == 2000)
                {
                    prow = null;
                }
                else
                {
                    prow = pcursor.NextRow();
                }
            }
            return pdatatable;
        }

        //获得合法的表名。因为ＤＡＴＡＴＡＢＬＥ的表名不允许含有“．”，因此我们用＂_＂替换．
        public static string getvalidfeatureclassname(string fcname)
        {
            int dot = fcname.IndexOf(".");
            if (dot != -1)
            {
                return fcname.Replace(".", "_");
            }
            return fcname;

        }
    }
}
