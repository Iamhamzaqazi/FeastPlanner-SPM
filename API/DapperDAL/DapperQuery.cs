using AppDBContext.General;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DapperDAL
{
    public class DapperQuery
    {
        public string FormatSelectQuery<T>(string Clause)
        {
            string FormattedQuery = "";
            try
            {
                Type type = typeof(T);
                string ModelPropertyName = "";
                foreach (var PropertyName in type.GetProperties().Where(x => !x.PropertyType.FullName.Contains("AppDBContext.Models.") && !Attribute.IsDefined(x, typeof(NotMappedAttribute))))
                {
                    ModelPropertyName += $@"""{PropertyName.Name}""," + "\n";
                }
                int lastCommaIndex = ModelPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    ModelPropertyName = ModelPropertyName.Substring(0, lastCommaIndex) + ModelPropertyName.Substring(lastCommaIndex + 1);
                }
                FormattedQuery = $@"Select{"\n"}{ModelPropertyName}{"\n"}from ""{type.Name}""{"\n"} Where 1=1";
                FormattedQuery = FormattedQuery.ToUpper();
                FormattedQuery += $@"{Clause}";
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return FormattedQuery;
        }

        //Step1
        public string FormatMergeQueryCreateTempTable<T>()
        {
            string FormattedQuery = "";
            try
            {
                Type type = typeof(T);
                int TotalCount = type.GetProperties().Where(x => !x.PropertyType.FullName.Contains("AppDBContext.Models.") && !Attribute.IsDefined(x, typeof(NotMappedAttribute))).Count();
                int CurrentCount = 1;
                string ModelPropertyName = "";

                foreach (var PropertyInfo in type.GetProperties().Where(x => !x.PropertyType.FullName.Contains("AppDBContext.Models.") && !Attribute.IsDefined(x, typeof(NotMappedAttribute))))
                {

                    if (PropertyInfo.PropertyType == typeof(int) || PropertyInfo.PropertyType == typeof(Nullable<int>) || PropertyInfo.PropertyType == typeof(Int64) || PropertyInfo.PropertyType == typeof(Nullable<Int64>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} int null," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(decimal) || PropertyInfo.PropertyType == typeof(Nullable<decimal>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} numeric(19, 6) null," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(double) || PropertyInfo.PropertyType == typeof(Nullable<double>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} numeric(19, 6) null," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(string))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} nvarchar(150) null," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(DateTime) || PropertyInfo.PropertyType == typeof(Nullable<DateTime>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} datetime null," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(bool) || PropertyInfo.PropertyType == typeof(Nullable<bool>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} bit null," + "\n";
                    }
                }
                int lastCommaIndex = ModelPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    ModelPropertyName = ModelPropertyName.Substring(0, lastCommaIndex) + ModelPropertyName.Substring(lastCommaIndex + 1);
                }

                FormattedQuery = $@"Create Table #TempData{type.Name}Update {"\n"}( {ModelPropertyName} );{"\n"}";
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return FormattedQuery;
        }

        //Step2
        public string FormatMergeQueryInsertIntoTempTable<T>(bool IsArray, bool IsDocEntryMapped)
        {
            string FormattedQuery = "";
            try
            {
                Type type = typeof(T);
                string ModelPropertyName = "";
                string TempPropertyName = "";

                foreach (var PropertyInfo in type.GetProperties().Where(x => !x.PropertyType.FullName.Contains("AppDBContext.Models.") && !Attribute.IsDefined(x, typeof(NotMappedAttribute))))
                {
                    if (PropertyInfo.PropertyType == typeof(int) || PropertyInfo.PropertyType == typeof(Nullable<int>) || PropertyInfo.PropertyType == typeof(Int64) || PropertyInfo.PropertyType == typeof(Nullable<Int64>))
                    {
                        if (PropertyInfo.Name.ToLower() == $@"docentry" && IsArray)
                        {
                            ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                            TempPropertyName += $@"@DOCENTRY," + "\n";
                        }
                        else
                        {
                            ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                            TempPropertyName += $@"t.c.value('({PropertyInfo.Name})[1]', 'INT') as {PropertyInfo.Name}," + "\n";
                        }
                    }
                    if (PropertyInfo.PropertyType == typeof(decimal) || PropertyInfo.PropertyType == typeof(Nullable<decimal>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                        TempPropertyName += $@"t.c.value('({PropertyInfo.Name})[1]', 'numeric(19,6)') as {PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(double) || PropertyInfo.PropertyType == typeof(Nullable<double>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                        TempPropertyName += $@"t.c.value('({PropertyInfo.Name})[1]', 'numeric(19,6)') as {PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(string))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                        TempPropertyName += $@"t.c.value('({PropertyInfo.Name})[1]', 'nvarchar(150)') as {PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(DateTime) || PropertyInfo.PropertyType == typeof(Nullable<DateTime>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                        TempPropertyName += $@"t.c.value('({PropertyInfo.Name})[1]', 'datetime') as {PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(bool) || PropertyInfo.PropertyType == typeof(Nullable<bool>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name}," + "\n";
                        TempPropertyName += $@"t.c.value('({PropertyInfo.Name})[1]', 'bit') as {PropertyInfo.Name}," + "\n";
                    }
                }
                int lastCommaIndex = ModelPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    ModelPropertyName = ModelPropertyName.Substring(0, lastCommaIndex) + ModelPropertyName.Substring(lastCommaIndex + 1);
                }
                lastCommaIndex = TempPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    TempPropertyName = TempPropertyName.Substring(0, lastCommaIndex) + TempPropertyName.Substring(lastCommaIndex + 1);
                }
                if (IsArray && IsDocEntryMapped == false)
                {
                    FormattedQuery = $@"{"\n"}Insert Into #TempData{type.Name}Update ( {ModelPropertyName} ){"\n"}Select {TempPropertyName}{"\n"}From @{type.Name}.nodes('/ArrayOf{type.Name}/{type.Name}') as t(c);";
                }
                else if (IsDocEntryMapped && IsArray == false)
                {
                    FormattedQuery = $@"Insert Into #TempData{type.Name}Update ( {ModelPropertyName} ){"\n"}Select {TempPropertyName}{"\n"}From @{type.Name}.nodes('/{type.Name}') as t(c);";
                    FormattedQuery += $@"{"\n"}Declare @DOCENTRY as INT if (Select ""DocEntry"" from #TempData{type.Name}Update) = 0 BEGIN set @DOCENTRY = (SELECT ISNULL((MAX(""DocEntry"")),0)+1 as ""DOCENTRY"" FROM ""{type.Name}""); END ELSE BEGIN SET @DOCENTRY = (Select ""DocEntry"" from #TempData{type.Name}Update); END";
                }
                else if (IsArray && IsDocEntryMapped)
                {
                    FormattedQuery = $@"{"\n"}Insert Into #TempData{type.Name}Update ( {ModelPropertyName} ){"\n"}Select {TempPropertyName}{"\n"}From @{type.Name}.nodes('/ArrayOf{type.Name}/{type.Name}') as t(c);";
                }
                else
                {
                    FormattedQuery = $@"Insert Into #TempData{type.Name}Update ( {ModelPropertyName} ){"\n"}Select {TempPropertyName}{"\n"}From @{type.Name}.nodes('/{type.Name}') as t(c);";
                }

            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return FormattedQuery;
        }

        //Step3
        public string FormatMergeQueryCreatePropertiesName<T>()
        {
            string FormattedQuery = "";
            try
            {
                Type type = typeof(T);
                string ModelPropertyName = "";

                foreach (var PropertyInfo in type.GetProperties().Where(x => !x.PropertyType.FullName.Contains("AppDBContext.Models.") && !Attribute.IsDefined(x, typeof(NotMappedAttribute))))
                {
                    if (PropertyInfo.Name.ToLower() == "id")
                    {
                        continue;
                    }
                    ModelPropertyName += $@"{PropertyInfo.Name},";
                }
                int lastCommaIndex = ModelPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    ModelPropertyName = ModelPropertyName.Substring(0, lastCommaIndex) + ModelPropertyName.Substring(lastCommaIndex + 1);
                }
                FormattedQuery = $@"{ModelPropertyName}";
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return FormattedQuery;
        }

        //Step4
        public string FormatMergeQuerySetProperties<T>(bool RunDeleteCommand, bool IsRole)
        {
            string FormattedQuery = "";
            try
            {
                Type type = typeof(T);
                string ModelPropertyName = "";
                string SourceModelPropertyName = "";

                foreach (var PropertyInfo in type.GetProperties().Where(x => !x.PropertyType.FullName.Contains("AppDBContext.Models.") && !Attribute.IsDefined(x, typeof(NotMappedAttribute))))
                {
                    if (PropertyInfo.Name.ToLower() == "id")
                    {
                        continue;
                    }

                    if (PropertyInfo.PropertyType == typeof(int) || PropertyInfo.PropertyType == typeof(Nullable<int>) || PropertyInfo.PropertyType == typeof(Int64) || PropertyInfo.PropertyType == typeof(Nullable<Int64>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} = source.{PropertyInfo.Name}," + "\n";
                        if (PropertyInfo.Name.ToLower() == $@"docentry")
                        {
                            SourceModelPropertyName += $@"@DOCENTRY," + "\n";
                        }
                        else
                        {
                            SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                        }
                    }
                    if (PropertyInfo.PropertyType == typeof(decimal) || PropertyInfo.PropertyType == typeof(Nullable<decimal>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} = source.{PropertyInfo.Name}," + "\n";
                        SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(double) || PropertyInfo.PropertyType == typeof(Nullable<double>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} = source.{PropertyInfo.Name}," + "\n";
                        SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(string))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} = source.{PropertyInfo.Name}," + "\n";
                        SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                    }
                    if (PropertyInfo.PropertyType == typeof(DateTime) || PropertyInfo.PropertyType == typeof(Nullable<DateTime>))
                    {
                        if (PropertyInfo.Name.Equals("UpdatedDt", StringComparison.OrdinalIgnoreCase))
                        {
                            // On update: use GETDATE()
                            ModelPropertyName += $@"{PropertyInfo.Name} = GETDATE()," + "\n";

                            // On insert: still use source value
                            SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                        }
                        else
                        {
                            ModelPropertyName += $@"{PropertyInfo.Name} = source.{PropertyInfo.Name}," + "\n";
                            SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                        }
                    }
                    if (PropertyInfo.PropertyType == typeof(bool) || PropertyInfo.PropertyType == typeof(Nullable<bool>))
                    {
                        ModelPropertyName += $@"{PropertyInfo.Name} = source.{PropertyInfo.Name}," + "\n";
                        SourceModelPropertyName += $@"source.{PropertyInfo.Name}," + "\n";
                    }
                }
                int lastCommaIndex = ModelPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    ModelPropertyName = ModelPropertyName.Substring(0, lastCommaIndex) + ModelPropertyName.Substring(lastCommaIndex + 1);
                }
                lastCommaIndex = SourceModelPropertyName.LastIndexOf(',');
                if (lastCommaIndex >= 0)
                {
                    SourceModelPropertyName = SourceModelPropertyName.Substring(0, lastCommaIndex) + SourceModelPropertyName.Substring(lastCommaIndex + 1);
                }
                if (RunDeleteCommand)
                {
                    FormattedQuery = $@"{ModelPropertyName}{"\n"}WHEN NOT MATCHED BY target THEN Insert{"\n"}({FormatMergeQueryCreatePropertiesName<T>()}){"\n"}Values{"\n"}({SourceModelPropertyName}){"\n"}{"\n"}WHEN NOT MATCHED BY source and target.DocEntry = @DOCENTRY THEN DELETE;{"\n"}DROP TABLE #TempData{type.Name}Update;{"\n"}{"\n"}";
                }
                else
                {
                    if (IsRole)
                    {
                        FormattedQuery = $@"{ModelPropertyName}{"\n"}WHEN NOT MATCHED BY target THEN Insert{"\n"}({FormatMergeQueryCreatePropertiesName<T>()}){"\n"}Values{"\n"}({SourceModelPropertyName});{"\n"}SELECT Id FROM #TempDataMstRoleUpdate; {"\n"} DROP TABLE #TempData{type.Name}Update;{"\n"}{"\n"}";
                    }
                    else
                    {
                        FormattedQuery = $@"{ModelPropertyName}{"\n"}WHEN NOT MATCHED BY target THEN Insert{"\n"}({FormatMergeQueryCreatePropertiesName<T>()}){"\n"}Values{"\n"}({SourceModelPropertyName});{"\n"}DROP TABLE #TempData{type.Name}Update;{"\n"}{"\n"}";
                    }
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return FormattedQuery;
        }

        //Step5
        public string FormatMergeQuery<T>(bool IsArray, bool RunDeleteCommand, bool IsDocEntryMapped, bool IsRole)
        {
            string FormattedQuery = "";
            try
            {
                Type type = typeof(T);
                if (IsRole)
                {
                    FormattedQuery = $@"DECLARE @OutputId INT;{"\n"}";
                }
                if (IsDocEntryMapped)
                {
                    FormattedQuery += $@"{FormatMergeQueryCreateTempTable<T>()}{FormatMergeQueryInsertIntoTempTable<T>(IsArray, IsDocEntryMapped)}{"\n"}{"\n"}MERGE INTO ""{type.Name.ToUpper()}"" AS target{"\n"}USING #TempData{type.Name}Update AS source ON target.Id = source.Id and target.DocEntry = source.DocEntry{"\n"}{"\n"}WHEN MATCHED THEN Update Set{"\n"}{FormatMergeQuerySetProperties<T>(RunDeleteCommand, IsRole)}";
                }
                else
                {
                    FormattedQuery += $@"{FormatMergeQueryCreateTempTable<T>()}{FormatMergeQueryInsertIntoTempTable<T>(IsArray, IsDocEntryMapped)}{"\n"}{"\n"} MERGE INTO ""{type.Name.ToUpper()}"" AS target{"\n"}USING #TempData{type.Name}Update AS source ON target.Id = source.Id{"\n"}{"\n"}WHEN MATCHED THEN Update Set{"\n"}{FormatMergeQuerySetProperties<T>(RunDeleteCommand, IsRole)}";
                }
            }
            catch (Exception ex)
            {
                LogsAPI.GenerateLogs(ex);
            }
            return FormattedQuery;
        }

    }
}