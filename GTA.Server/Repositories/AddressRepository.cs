using GTA.Data.Models;

namespace GTA.Server.Repositories {
    public class AddressRepository : Repository<Address, Controller<Address>>{
        public AddressRepository(Context context) : base(context) { }
    }
}
