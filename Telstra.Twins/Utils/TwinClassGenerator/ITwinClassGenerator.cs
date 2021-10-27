namespace Telstra.Twins.Utils.TwinClassGenerator
{
    public interface ITwinClassGenerator
    {
        public string FromDTDL(string dtdl);
        public string FromModel(DTDLModel model);
    }
}
