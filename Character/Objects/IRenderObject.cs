using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Character.Objects
{
    internal interface IRenderObject
    {
        public long Tick { get; set; }
        public int Action { get; set; }
        public void OnLoad(Shader shader);
        public void OnRenderFrame(Shader shader);
        public void OnUnload();
    }
}
