using SalemCartographer.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalemCartographer.App
{
  class SessionMatcher
  {
    private static SessionMatcher _Instance;
    public static SessionMatcher Instance {
      get {
        if (_Instance == null) {
          _Instance = new SessionMatcher();
        }
        return _Instance;
      }
    }

    protected SessionMatcher() {

    }

    public void addHash(TileDto hash) {

    }
    
    public void addHashs(AreaDto hash) {

    }
    
    public void addHashs(WorldDto hash) {

    }



  }
}
