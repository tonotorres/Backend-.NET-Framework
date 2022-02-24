using EPICX_ORACLE_SEARCH.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace EPICX_ORACLE_SEARCH.Repositories
{
    public class Repository_EPICX
    {
        static List<Registro> registros = LlenarRegistros();
        static public List<String> GetSchemas()
        {
            string pathToFiles = HttpContext.Current.Server.MapPath("/App_Data");

            DirectoryInfo d = new DirectoryInfo(pathToFiles);

            FileInfo[] Files = d.GetFiles("*.json");

            List<String> namesSchemas = new List<string>();
            foreach (FileInfo file in Files)
            {
                string filename = Path.GetFileNameWithoutExtension(file.Name);
                if (!filename.Contains("_formatted"))
                    namesSchemas.Add(filename);
            }
            return namesSchemas;
        }

        static public List<String> GetTablesNames(string schema)
        {
            string pathToFiles = HttpContext.Current.Server.MapPath("/App_Data");
            List<String> tablesNames = new List<string>();
            using (StreamReader file = File.OpenText(pathToFiles + "\\" + schema + "_formatted.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o = (JObject)JToken.ReadFrom(reader);
                foreach (var item in o)
                {
                    Console.WriteLine(item.Key);
                    String name = item.Key;
                    var value = item.Value.First.First;
                    String nombre = value.ToString();
                    nombre = nombre.Replace("\"", "");
                    nombre = nombre.Split(':')[0];
                    if (name != nombre)
                    {
                        name = nombre;
                    }
                    //name = nombre;
                    tablesNames.Add(name);
                    //var nombreTabla = value.;
                }
                //var variable = o[];
                Console.WriteLine("");
            }
            return tablesNames;
        }

        static public List<String> GetTablesNamesRegistro(string schema)
        {
            List<String> tablas = registros.Where(r => r.Schema == schema).Select(y => y.Tabla).OrderBy(x => x).Distinct().ToList();
            return tablas;
        }

        static public List<String> GetCamposNames(string schema, string tablename)
        {
            string pathToFiles = HttpContext.Current.Server.MapPath("/App_Data");
            List<String> campos = new List<string>();
            using (StreamReader file = File.OpenText(pathToFiles + "\\" + schema + "_formatted.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o = (JObject)JToken.ReadFrom(reader);
                //Cogo la tabla
                var tabla = o.SelectToken(tablename);
                foreach (var item in tabla)
                {
                    //var campo = item[tabla];
                    var value = (string)item.First.First;
                    campos.Add(value);
                    //if (campo != null)
                    //    campos.Add(campo.ToString());
                    ////var campo = item.SelectToken(tablename);
                }
            }
            return campos;

        }

        static public List<String> GetCamposNamesRegistro(string schema, string tablename)
        {
            List<String> campos = registros.Where(x => x.Schema == schema && x.Tabla == tablename).Select(y => y.Campo).Distinct().ToList();
            return campos;
        }


        static public List<String> GetApplications()
        {
            List<String> aplicaciones = registros.Select(r => r.Aplicacion).OrderBy(x => x).Distinct().ToList();
            return aplicaciones;
        }

        static public JObject GetQuerys(string schema, string tabla, string[] campos)
        {

            List<Registro> finales = new List<Registro>();
            if (campos[0] != "0")
            {
                finales = registros.Where(r => r.Schema == schema && r.Tabla == tabla && campos.Contains(r.Campo)).OrderBy(x => x.Aplicacion).ToList();
            }
            else
            {
                finales = registros.Where(r => r.Schema == schema && r.Tabla == tabla).OrderBy(x => x.Aplicacion).ToList();
            }

            JObject rss =
                new JObject(
                    new JProperty("schema", schema),
                    new JProperty("tables",
                        new JObject(
                            new JProperty("name", tabla),
                            new JProperty("values",
                                new JArray(
                                    from p in finales
                                    orderby p.Campo
                                    select new JObject(
                                        new JProperty("field", p.Campo),
                                        new JProperty("project", p.Aplicacion),
                                        new JProperty("value", p.Query)))))));
            return rss;
        }

        static public JObject GetQuerysApplication(String application)
        {
            //Filtro por aplicacion
            List<Registro> querysApl = registros.Where(x => x.Aplicacion == application).ToList();
            List<Registro> todo = registros.ToList();

            List<String> schemasNames = querysApl.Select(x => x.Schema).Distinct().ToList();

            List<Schema> schemasFitrados = new List<Schema>();
            foreach (var nameSchema in schemasNames)
            {
                //Busco por schema
                List<Registro> registroSchema = querysApl.Where(x => x.Schema == nameSchema).ToList();

                //Creo modelo de schema
                Schema schema = new Schema();
                schema.schema = nameSchema;
                schema.tables = new List<Table>();

                List<String> tablasAfectadas = registroSchema.Select(x => x.Tabla).OrderBy(x => x).Distinct().ToList();

                //Debo recorrer por tabla
                foreach (var r in tablasAfectadas)
                {
                    //Busco los registros por una tabla
                    List<Registro> registroTabla = registroSchema.Where(x => x.Tabla == r).ToList();
                    //List<Registro> registroTablaDistinct = registroSchema.Where(x => x.Tabla == r).Distinct().ToList();

                    //Listado de campos por la tabla
                    Table tabla = new Table();
                    tabla.table = r;
                    tabla.values = new List<Query>();

                    //List<Query> queries = new List<Query>();
                    //Recorro los campos
                    foreach (var campo in registroTabla)
                    {
                        //Creo elemento de query
                        Query query = new Query
                        {
                            field = campo.Campo,
                            value = campo.Query
                        };
                        tabla.values.Add(query);

                    }
                    schema.tables.Add(tabla);
                }
                schemasFitrados.Add(schema);
            }
            string json = JsonConvert.SerializeObject(schemasFitrados, Formatting.None);
            JArray jA = JArray.FromObject(schemasFitrados);
            JObject jObj = new JObject();
            jObj.Add(new JProperty("project", application));
            jObj.Add(new JProperty("schemas", jA));

            //LlenarRegistros();

            return jObj;
        }

        static private List<Registro> LlenarRegistros()
        {
            List<String> schemas = GetSchemas();
            List<Registro> Registros = new List<Registro>();

            FormartJson();

            foreach (var schema in schemas)
            {
                string pathToFiles = HttpContext.Current.Server.MapPath("/App_Data");


                using (StreamReader file = File.OpenText(pathToFiles + "\\" + schema + "_formatted.json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    foreach (var item in o)
                    {
                        Console.WriteLine(item.Key);
                        String name = item.Key;
                        var value = item.Value.First.First;
                        String nombre = value.ToString();
                        nombre = nombre.Replace("\"", "");
                        nombre = nombre.Split(':')[0];
                        if (name != nombre)
                        {
                            name = nombre;
                        }
                        var nodosDatos = item.Value;
                        foreach (var nodo in nodosDatos)
                        {
                            //Guardo en un array
                            var todo = nodo.ToArray();
                            //Saco la tabla y el campo
                            var tablaCampo = todo[0].ToString();
                            tablaCampo = tablaCampo.Replace("\"", "");
                            tablaCampo = tablaCampo.Replace(" ", "");
                            var tablaCampoList = tablaCampo.Split(':');
                            String tabla = tablaCampoList[0];
                            String campo = tablaCampoList[1];

                            for (int i = 1; i < todo.Length; i++)
                            {
                                //Creo modelo
                                Registro registro = new Registro();
                                registro.Schema = schema;

                                registro.Tabla = tabla;
                                registro.Campo = campo;
                                var aplQuery = todo[i].ToString();
                                aplQuery = aplQuery.Replace("\"", "");
                                aplQuery = aplQuery.Replace(" ", "");
                                var aplQueryList = aplQuery.Split(':');
                                String aplicacion = aplQueryList[0];
                                String query = aplQueryList[1];

                                //Meto datos a el registro y lo guardo en listado
                                registro.Aplicacion = aplicacion;
                                registro.Query = query;
                                Registros.Add(registro);
                            }
                        }
                    }
                }
            }

            return Registros;
        }

        static private void FormartJson()
        {
            List<String> schemas = GetSchemas();

            foreach (var schema in schemas)
            {
                string pathToFiles = HttpContext.Current.Server.MapPath("/App_Data");
                using (StreamReader file = File.OpenText(pathToFiles + "\\" + schema + ".json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);

                    //Limpio el json de datos vacios
                    string json = o.ToString();
                    json = o.ToString(Formatting.None);
                    json = json.Replace(",{}", "");

                    //vuelvo a guardarlo como JObject
                    o = JObject.Parse(json);
                    File.WriteAllText(pathToFiles + "\\" + schema + "_formatted.json", o.ToString());
                }
            }
        }
    }
}