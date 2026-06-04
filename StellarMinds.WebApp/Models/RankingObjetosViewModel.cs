using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Models
{
    // RF10 - Ranking de objetos celestes observados (cualquier rol).
    public class RankingObjetosViewModel
    {
        public List<RankingObjetoCelesteModel> Ranking { get; set; } = new();
    }
}
