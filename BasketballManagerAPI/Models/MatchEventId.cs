using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public enum MatchEventId : int {
        OnePointHit = 1,
        OnePointMiss = 2,
        TwoPointHit = 3,
        TwoPointMiss = 4,
        ThreePointHit = 5,
        ThreePointMiss = 6,
        Assist = 7,
        OffensiveRebound = 8,
        DefensiveRebound = 9,
        Steal = 10,
        Block = 11,
        Turnover = 12
    }

}
