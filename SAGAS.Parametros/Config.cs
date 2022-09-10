using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SAGAS.Parametros
{
   public class Config
    {

        public const string MagicWord = "olrfjhsd3470jkcza/;114p";

       
        public static Configuration config = ConfigurationManager.OpenExeConfiguration("SAGAS.config");

        public static Configuration Configuration
        {
            get { return Config.config; }
            set { Config.config = value; }
        }

        /// <summary>
        /// Significado: Nombre de la Computadora
        /// </summary>
        public const string strVarNamePC = "KeyNamePC";
        /// <summary>
        /// Significado: Serial de la Tarjeta Madre
        /// </summary>
        public const string strVarBaseBoard = "KeyBaseBoard";
        /// <summary>
        /// Significado: Serial del Disco Duro
        /// </summary>
        public const string strVarSerialDisk = "KeySerialDisk";
        /// <summary>
        /// Significado: Serial CPU Actual
        /// </summary>
        public const string strVarCPUActual = "KeyCPUActual";
        /// <summary>
        /// Significado: Serial Logico del Disco Duro
        /// </summary>
        public const string strLogicalDiskSerial = "KeyLogicalDiskSerial";
        /// <summary>
        /// Significado: Estación donde fue instalado
        /// </summary>
        public const string strVarESID = "KeyESID";
        /// <summary>
        /// Significado: Tipo de conexión
        /// </summary>
        public const string strTipoConexionID = "KeyTipoConexionID";
        /// <summary>
        /// Significado: Estación donde fue instalado
        /// </summary>
        public const string strVarKeyRegistro = "KeyRegistro";
        /// <summary>
        /// Significado: Tipo de conexión
        /// </summary>
        public const string strKeyBD = "KeyBD";
        /// <summary>
        /// Significado: Usuario Registrado
        /// </summary>
        public const string strUserRegistered = "KeyUserRegistered";
        /// <summary>
        /// Significado: Fecha Update
        /// </summary>
        public const string strUpdateDate = "KeyUpdateDate";
        /// <summary>
        /// Significado: Impresora
        /// </summary>
        public const string strPrinterLocal = "KeyPrinterLocal";

        /// <summary>
        /// Metodo que retorna el valor de la configuracion del key dentro del appsettings 
        /// </summary>
        /// <param name="KeyAppSettings">string del nombre del Key registrado en las configuraciones del AppSettings</param>
        public static string GetValueByKeyAppSettings(string KeyAppSettings)
        {
            try
            {
                //config = ConfigurationManager.OpenExeConfiguration("SAGAS.config");

                string KeyAppSettingsEncrypt = Security.Encrypt(KeyAppSettings, MagicWord);
                string Cnn = "";
                if (config.AppSettings.Settings.AllKeys.Contains(KeyAppSettingsEncrypt))
                {
                    Cnn = config.AppSettings.Settings[KeyAppSettingsEncrypt].Value;
                }

                return Security.Decrypt(Cnn, MagicWord);
            }
            catch
            { return ""; }
        }

        /// <summary>
        /// Metodo que establece y guarda la configuracion de cada key dentro del appsettings 
        /// </summary>
        /// <param name="KeyAppSettings">string del nombre del Key registrado en las configuraciones del AppSettings</param>
        /// <param name="ValueAppSettings">string del valor que se quiere asignar al KeyAppSettings</param>
        public static void SetValueByKeyAppSettings(string KeyAppSettings, string ValueAppSettings)
        {

            //config = ConfigurationManager.OpenExeConfiguration("SAGAS.config");
         
            string KeyAppSettingsEncrypt = Security.Encrypt(KeyAppSettings, MagicWord);
            string ValueAppSettingsEncrypt = Security.Encrypt(ValueAppSettings, MagicWord);

            if (config.AppSettings.Settings.AllKeys.Contains(KeyAppSettingsEncrypt))
            {
                config.AppSettings.Settings[KeyAppSettingsEncrypt].Value = ValueAppSettingsEncrypt;
                config.Save();
            }
            else
            {
                config.AppSettings.Settings.Add(KeyAppSettingsEncrypt, ValueAppSettingsEncrypt);
                config.Save();
            }
        }

        /// <summary>
        /// Metodo que retorna un string de la cadena de conexion
        /// </summary>
        /// <returns>(string)</returns>
        public static string GetCadenaConexionString()
        {
             try
            {
            //config = ConfigurationManager.OpenExeConfiguration("SAGAS.config");
            string Cnn = config.ConnectionStrings.ConnectionStrings["cnnSAGAS"].ConnectionString;
            return  Security.Decrypt(Cnn, MagicWord);
            }
            catch
            {
                return General.Conexion;
            }
        }

        /// <summary>
        /// Metodo que establece y guarda la cadena de conexion  
        /// </summary>
        /// <param name="Cadena">string de la cadena de conexión a la base de datos</param>
        public static void SetCadenaConexionString(string Cadena)
        {

            string securityEncrypt = Security.Encrypt(Cadena, MagicWord);
            //config = ConfigurationManager.OpenExeConfiguration("SAGAS.config");
            ConnectionStringsSection css = config.ConnectionStrings;
            css.ConnectionStrings["cnnSAGAS"].ConnectionString = securityEncrypt;
            config.Save();

        }

        public static bool TestConnection()
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
                {
                    conn.ConnectionString = Parametros.Config.GetCadenaConexionString();
                    conn.Open();
                    conn.Close();

                    return true;
                }
            }
            catch (System.Data.SqlClient.SqlException except)
            {
                switch (except.Number)
                {
                    case 26:
                        MessageBox.Show("NO HAY CONEXION EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 50:
                        MessageBox.Show("NO HAY CONEXION EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 53:
                        MessageBox.Show("NO HAY CONEXION EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 233:
                        MessageBox.Show("CONEXIÓN CON EL SERVIDOR ESTABLECIDA, PERO EL LOGIN ES INCORRECTO." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 4060:
                        MessageBox.Show("NO SE PUEDE CONECTARSE A LA BASE DE DATOS INDICADA." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        MessageBox.Show("ERROR GENERAL DE LA CONEXION DE SQL!!!" + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                return false;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "Error de conexión a la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

        }

        #region <<< CONFIG REGISTRO >>>

        public static string SelectedPrinterLocal
        {
            get
            {
                string TP = "";
                try
                {
                    if (!String.IsNullOrEmpty(Parametros.Config.GetValueByKeyAppSettings(Parametros.Config.strPrinterLocal)))
                        TP = Convert.ToString(Parametros.Config.GetValueByKeyAppSettings(Parametros.Config.strPrinterLocal));
                }
                catch { TP = ""; }

                return TP;
            }
        }

        #endregion

        #region <<< CONFIG DATA TABLE >>>

        public static bool DisplayMessageDialog()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToBoolean(db.Configuracions.Single(c => c.Campo == "DisplayMessageDialog").Valor);

                return Mp;
            }
            catch 
            {
                return false;
            }
        }

        public static int ProductoClaseCombustible()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ProductoClaseCombustible").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int RangoDiasPrecioCombustible()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "RangoDiasPrecioCombustible").Valor);

                return Mp;
            }
            catch
            {
                return 1;
            }
        }

        public static decimal VariacionPorcentualPrecio()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "VariacionPorcentualPrecio").Valor);

                return Mp;
            }
            catch
            {
                return 10;
            }
        }

        public static decimal DiferenciaVerificacionLectura()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "DiferenciaVerificacionLectura").Valor);

                return Mp;
            }
            catch
            {
                return 1m;
            }
        }

        public static decimal TipoCambioArqueo(DateTime fechaRD)
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = db.ArqueoTipoCambios.Where(c => c.Fecha.Date <= fechaRD.Date).OrderByDescending(o => o.Fecha);

                return Mp.First().TipoCambio;
            }
            catch
            {
                return 0m;
            }
        }

        public static decimal MargenDiferenciaCambiaria()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenDiferenciaCambiaria").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }

        public static int ProveedorPrestamoManejoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ProveedorPrestamoManejoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ClientePrestamoManejoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ClientePrestamoManejoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaAnticipoClienteID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaAnticipoClienteID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int MonedaPrincipal()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MonedaPrincipal").Valor);

                return Mp;
            }
            catch
            {
                return 1;
            }
        }

        public static int MonedaSecundaria()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MonedaSecundaria").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int IDFormaPagoEfectivo()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "IDFormaPagoEfectivo").Valor);

                return Mp;
            }
            catch
            {
                return 20;
            }
        }

        public static int ConceptoSalarioEmpleadoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ConceptoSalarioEmpleadoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int MovimientoHoraExtraID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MovimientoHoraExtraID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int MovimientoPensionID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MovimientoPensionID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }       

        public static int MovimientoAusenciaID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MovimientoAusenciaID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int MovimientoSubsidioID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MovimientoSubsidioID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int FormatoRetencion()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "FormatoRetencion").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int FormatoRetencionAlcaldia()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "FormatoRetencionAlcaldia").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal DescuentoPetroCard()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "DescuentoPetroCard").Valor);

                return Mp;
            }
            catch
            {
                return 1m;
            }
        }

        public static decimal RangoEfectivo()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "RangoEfectivo").Valor);

                return Mp;
            }
            catch
            {
                return 1m;
            }
        }

        public static decimal RangoMecElectronico()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "RangoMecElectronico").Valor);

                return Mp;
            }
            catch
            {
                return 1m;
            }
        }

        public static bool PermitirCombustibleEntradaInv()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToBoolean(db.Configuracions.Single(c => c.Campo == "PermitirCombustibleEntradaInv").Valor);

                return Mp;
            }
            catch
            {
                return false;
            }
        }

        public static decimal MargenAjusteVentasCreditoComb()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenAjusteVentasCreditoComb").Valor);

                return Mp;
            }
            catch
            {
                return 0.03m;
            }
        }
        public static int ConceptoCajaChicaID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ConceptoCajaChicaID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuponesNotaID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuponesNotaID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static string GetReportReady(string Campo)
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = (db.Configuracions.Single(c => c.Campo == Campo).Valor).ToString();

                return Mp;
            }
            catch
            {
                return "";
            }
        }

        public static String[] ListaTurno()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                String[] Mp = (db.Configuracions.Single(c => c.Campo == "ListaTurno").Valor).ToString().Split(',');

                return Mp;
            }
            catch
            {
                return null;
            }
        }

        public static String[] ListaEntregar()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                String[] Mp = (db.Configuracions.Single(c => c.Campo == "ListaEntregar").Valor).ToString().Split(',');

                return Mp;
            }
            catch
            {
                return null;
            }
        }

        #region <<< PARAMETROS_CONTABLES >>>

        public static decimal MargenToleranciaCosto()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenToleranciaCosto").Valor);

                return Mp;
            }
            catch
            {
                return 10m;
            }
        }

        public static decimal MargenValorCambioIVA()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenValorCambioIVA").Valor);

                return Mp;
            }
            catch
            {
                return 1m;
            }
        }

        public static decimal MargenValorCambioAlcaldia()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenValorCambioAlcaldia").Valor);

                return Mp;
            }
            catch
            {
                return 0.02m;
            }
        }

        public static decimal MargenValorCambioTC()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenValorCambioTC").Valor);

                return Mp;
            }
            catch
            {
                return 1m;
            }
        }

        public static int IVAPorPagar()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "IVAPorPagar").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }


        public static int IVAPorAcreditar()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "IVAPorAcreditar").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int IVAAcreditado()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "IVAAcreditado").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ProveedorCombustibleID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ProveedorCombustibleID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ProductoAreaServicioID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ProductoAreaServicioID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ConceptoInventarioInicial()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ConceptoInventarioInicial").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int TipoClienteManejoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "TipoClienteManejoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ClienteVentaConttadoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ClienteVentaContadoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ClienteVentaContadoMonedaExtrangeraID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ClienteVentaContadoMonedaExtrangeraID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ClienteDNPSubsidio()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ClienteDNPSubsidio").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }       

        public static int ProveedorNoRegistradoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ProveedorNoRegistradoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int TipoExtraccionManejoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "TipoExtraccionManejoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CajaMonedaNacional()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CajaMonedaNacional").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CajaMonedaExtranjera()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CajaMonedaExtranjera").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaPorCobrarEmpleado()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaPorCobrarEmpleado").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaSobranteArqueo()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaSobranteArqueo").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaAlcaldiaPorPagarID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaAlcaldiaPorPagarID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaCobrarCompaniaGrupo()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaCobrarCompaniaGrupo").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime InicioAguinaldo()
        {
            DateTime inicioAguinaldo = DateTime.Today;
            try
            {
                Entidad.SAGASDataClassesDataContext db = new SAGAS.Entidad.SAGASDataClassesDataContext(Config.GetCadenaConexionString());
                inicioAguinaldo = Convert.ToDateTime(db.Configuracions.Single(c => c.Campo == "FechaInicioAguinaldo").Valor);
                return inicioAguinaldo;
            }
            catch
            {
                return DateTime.Today;
            }
        }

        public static DateTime FinAguinaldo()
        {
            DateTime finAguinaldo = DateTime.Today;
            try
            {
                Entidad.SAGASDataClassesDataContext db = new SAGAS.Entidad.SAGASDataClassesDataContext(Config.GetCadenaConexionString());
                finAguinaldo = Convert.ToDateTime(db.Configuracions.Single(c => c.Campo == "FechaFinAguinaldo").Valor);
                return finAguinaldo;
            }
            catch
            {
                return DateTime.Today;
            }
        }

        public static int MovimientoVacacionID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MovimientoVacacionID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static bool PagoVacacionesPlanilla()
        {
            bool pagoVacacionesPlanilla = false;
            try
            {
                Entidad.SAGASDataClassesDataContext db = new SAGAS.Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                pagoVacacionesPlanilla = Convert.ToBoolean(db.Configuracions.Single(c => c.Campo == "PagoVacacionesEnPlanilla").Valor);
                return pagoVacacionesPlanilla;
            }
            catch
            {
                return false;
            }
        }

        public static int TipoPagoChequeID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "TipoPagoChequeID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int TipoPagoTransferenciaID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "TipoPagoTransferenciaID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaGananciaDiferencialCambiarioID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaGananciaDiferencialCambiarioID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime FechaCierreActa()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDateTime(db.Configuracions.Single(c => c.Campo == "FechaCierreActa").Valor);

                return Mp;
            }
            catch
            {
                return Convert.ToDateTime("2014/01/01");
            }
        }

        public static decimal PorcentajeComisionBancariaROC()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "PorcentajeComisionBancariaROC").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }

        public static decimal ValorINSSLaboral()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "ValorINSSLaboral").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }
              
        public static decimal ValorINSSPatronal()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "ValorINSSPatronal").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }

        public static decimal ValorINATEC()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "ValorINATEC").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }
       
        public static decimal MaximoSalarioINSS()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MaximoSalarioINSS").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }

        public static decimal PorcentajeManejoCuentaROC()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "PorcentajeManejoCuentaROC").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }

        public static int PorcentajeComisionBancariaROCCuentaContable()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "PorcentajeComisionBancariaROCCC").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int PorcentajeManejoCuentaROCCuentaContable()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "PorcentajeManejoCuentaROCCC").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ConceptoAnticipo()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ConceptoAnticipo").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ExtracionAutoconsumoID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ExtracionAutoconsumoID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ExtracionDonacionesID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ExtracionDonacionesID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int OficinaCentralID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "OficinaCentralID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaUtilidadEjerciocioID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaUtilidadEjerciocioID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int ZonaManaguaID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "ZonaManaguaID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CuentaAnticipoProveedorID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaAnticipoProveedorID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int MovimientoAltaActivoSinContabilidadID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "MovimientoAltaActivoSinContabilidadID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal MargenAjusteComprobanteArqueo()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToDecimal(db.Configuracions.Single(c => c.Campo == "MargenAjusteComprobanteArqueo").Valor);

                return Mp;
            }
            catch
            {
                return 0m;
            }
        }

        public static int CuentaPerdidaCambiariaID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CuentaPerdidaCambiariaID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CECOOficinaDiferencialID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CECOOficinaDiferencialID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static int CECOEstacionesDiferencialID()
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Configuracions.Single(c => c.Campo == "CECOEstacionesDiferencialID").Valor);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        public static string ChargeSignature(int ChargeID)
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = (from e in db.Empleados
                          join c in db.Cargo on e.CargoID equals c.ID
                          where c.ID == ChargeID
                          select e.Nombres + ' ' + e.Apellidos).SingleOrDefault().ToString();

                return Mp;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #endregion
        
        #region <<< MODULOS REPORTES SSRS >>>

        public static int GetModuloID(string ModuloName)
        {
            try
            {
                SAGAS.Entidad.SAGASDataClassesDataContext db;
                db = new SAGAS.Entidad.SAGASDataClassesDataContext(SAGAS.Parametros.Config.GetCadenaConexionString());

                var Mp = Convert.ToInt32(db.Modulos.Single(c => c.Nombre == ModuloName).ID);

                return Mp;
            }
            catch
            {
                return 0;
            }
        }

        #endregion
        /// <summary>
        /// Valida formularios abiertos
        /// </summary>
        /// <param name="formName">string del nombre del formulario que está pendiente de abrir</param>
        /// <param name="Partentform">Form del formulario principal que contiene un arreglo de MDIChildren (Form[] MDIChildren)</param>
        public static bool FormCargado(string formName, Form Partentform)
        {
            foreach (System.Windows.Forms.Form form in Partentform.MdiChildren)
            {
                if (form.Name == formName)
                {
                    form.Activate();
                    return true;
                }
            }
            return false;
        }
    }
}
