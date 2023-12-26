namespace GTA.Data.Models {
    public class VaultReference : Model {
        public string ModelType { get; set; } = default!;
        public int ModelID { get; set; }
        public string Provider { get; set; } = default!;
        public string ProviderReference { get; set; } = default!;

    }
}
