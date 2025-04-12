using System.Collections.Generic;

namespace Worker.Domain.Entities.Autenticacao
{
    public class ApplicationSettings
    {
        public Auth Auth { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
        public StorageSettings StorageSettings { get; set; }
        public LogSettings LogSettings { get; set; }
        public OriginsCorsSettings OriginsCorsSettings { get; set; }
        public ExternalApiSettings ExternalApiSettings { get; set; }
        public CargaConfig CargaConfig { get; set; }
        public CargaSysConfig CargaSysConfig { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public ExpurgoConfig ExpurgoConfig { get; set; }
        public ExpurgoStorageConfig ExpurgoStorageConfig { get; set; }
        public ConfigIdentity ConfigIdentity { get; set; }
        public PerfilSettings PerfilSettings { get; set; }
        public ProcessBackSettings ProcessBackSettings { get; set; }
    }

    public class Auth
    {
        public string UrlAutentication { get; set; }
        public string JWTSecret { get; set; }
        public int Expiration { get; set; }
        public string ClientId { get; set; }
        public List<string> Scopes { get; set; }
    }
    public class DatabaseSettings
    {
        public string ConnectionStrings { get; set; }
    }
    public class StorageSettings
    {
        public string Uri { get; set; }
        public string Folder { get; set; }
        public string UriUploadLaudo { get; set; }
        public string UriExtract { get; set; }
        public string UriUnziped { get; set; }
        public string UriUploadMonitoria { get; set; }
        public string UriStorageAudio { get; set; }
    }
    public class LogSettings
    {
        public string Log { get; set; }
        public string SeriLogPath { get; set; }
    }

    public class OriginsCorsSettings
    {
        public string[] UrlOrigins { get; set; }
    }

    public class ExternalApiSettings
    {
        public string RecaptchaSecretKey { get; set; }
        public string RecaptchaUrl { get; set; }
    }
    public class CargaConfig
    {
        public string PathStorage { get; set; }
        public string PathRaiz { get; set; }
        public List<string> FtpsOrigem { get; set; }
        public string PathDestino { get; set; }
        public string PathVirtual { get; set; }
        public string ConfigPathIndex { get; set; }
        public string ConfigPathIndex_Pendente { get; set; }
        public string ConfigPathAudio { get; set; }
        public string ConfigPathAudio_Pendente { get; set; }
        public string ConfigPathSys { get; set; }
        public string ConfigPathSys_Pendente { get; set; }
        public bool Preload { get; set; }
        public bool Load { get; set; }
        public bool EmailLog { get; set; }
        public bool VerificarCargaAtrasada { get; set; }
        public int HoraInicioExecucao { get; set; }
        public int DelayEntreCargaEAudio { get; set; }
        public int DelayTesteConexao { get; set; }
        public int DelayTestePastaEmUso { get; set; }
        public int DelayPingApp { get; set; }
        public string PathAnexoLogs { get; set; }
        public int MonitorPing { get; set; }
    }

    public class CargaSysConfig
    {
        public string PathRaiz { get; set; }
        public List<string> FtpsOrigem { get; set; }
        public string PathDestino { get; set; }
        public string ConfigPathSys { get; set; }
        public string ConfigPathSys_Pendente { get; set; }
        public bool Preload { get; set; }
        public bool Load { get; set; }
        public int HoraInicioExecucao { get; set; }
        public int DelayTesteConexao { get; set; }
        public int DelayTestePastaEmUso { get; set; }
        public int DelayPingApp { get; set; }
    }

    public class EmailSettings
    {
        public string UrlServer { get; set; }
        public string PortServer { get; set; }
        public string EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailFrom { get; set; }
    }

    public class ExpurgoConfig
    {
        public string PathRaiz { get; set; }
        public List<string> FtpsOrigem { get; set; }
        public string PathDestino { get; set; }
        public string ConfigPathIndex { get; set; }
        public string ConfigPathIndex_Processados { get; set; }
        public string ConfigPathAudio { get; set; }
        public string ConfigPathAudio_Processados { get; set; }
        public string ConfigPathAudio_Divergentes { get; set; }
        public string ConfigPathSys { get; set; }
        public string ConfigPathSys_Processados { get; set; }
        public int HoraInicioExecucao { get; set; }
        public int DelayPingApp { get; set; }
        public string ConfigPathLogExpurgo { get; set; }
        public int DiasExpurgo { get; set; }
    }

    public class ExpurgoStorageConfig
    {
        public bool Ligado { get; set; }
        public bool LigadoLaudo { get; set; }
        public int DelayPingApp { get; set; }
        public string PastaBaseExpurgo { get; set; }
        public string PastaBaseExpurgoLaudo { get; set; }
        public int DiaMovimentacao { get; set; }
        public int DiaDelecao { get; set; }
        public int DiaDelecaoLaudo { get; set; }
        public int DelayTesteConexao { get; set; }
        public List<string> ProtegerPastaBase { get; set; }
        public string FolderStorage { get; set; }
        public string FolderVirtual { get; set; }
        public int QtdAnos { get; set; }
        public int QtdDias { get; set; }
    }

    public class ConfigIdentity
    {
        public bool RequireDigit { get; set; }
        public bool RequireLowerCase { get; set; }
        public bool RequireUpperCase { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public int RequiredUniqueChars { get; set; }
        public int RequiredLength { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
        public bool AllowedForNewUsers { get; set; }
        public double DefaultLockoutTimeSpan { get; set; }
        public string AllowedUserNameCharacters { get; set; }
        public bool RequireUniqueEmail { get; set; }
        public string SpecialCharactersAcept { get; set; }
    }

    public class PerfilSettings
    {
        public int[] PerfilAdministradorSystem { get; set; }
        public int[] PerfilAcessMain { get; set; }
        public int TempoLogadoResposta { get; set; }
    }

    public class ProcessBackSettings
    {
        public string PathDownload { get; set; }
    }
}