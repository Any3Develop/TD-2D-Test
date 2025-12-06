using System.Collections.Generic;
using Common.Repositiory;

namespace Gameplay.Waves
{
    public class WavesRepository : Repository<WaveConfig>
    {
        public WavesRepository(IEnumerable<WaveConfig> waves)
        {
            AddRange(waves);
        }
        
        public WaveConfig Get(int level)
        {
            return Find(x=> x.Level == level);
        }
    }
}