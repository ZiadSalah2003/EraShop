using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraShop.API.Contracts.Files
{
    public record FileUploadResponse(
        string Url,
        string PublicId,
        string SecureUrl,
        string Format
    );
}
