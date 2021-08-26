using System.Collections.Generic;
using System.Linq;
using CRepublic.Magic.Files;
using CRepublic.Magic.Files.CSV_Helpers;
using CRepublic.Magic.Files.CSV_Logic;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace CRepublic.Magic.Logic.Manager
{
    internal class GameObjectManager
    {
        public GameObjectManager(Level l)
        {
            this.Level                = l;
            this.GameObjects          = new List<List<GameObject>>();
            GameObjectRemoveList = new List<GameObject>();
            this.GameObjectsIndex     = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                this.GameObjects.Add(new List<GameObject>());
                this.GameObjectsIndex.Add(0);
            }
            this.ComponentManager     = new ComponentManager(this.Level);
		}

        internal ComponentManager ComponentManager;
        internal List<GameObject> GameObjectRemoveList;
        internal List<List<GameObject>> GameObjects;
        internal List<int> GameObjectsIndex;
        internal Level Level;

		public void AddGameObject(GameObject go)
        {
            if (go.ClassId <= 6 || go.ClassId == 8)
            {
                go.GlobalId = GenerateGameObjectGlobalId(go);
                if (go.ClassId == 0)
                {
                    var b = (Building) go;
                    var bd = b.GetBuildingData;
                    if (bd.IsWorkerBuilding())
                        this.Level.VillageWorkerManager.IncreaseWorkerCount();
                }
            }
            else
            {
                go.GlobalId = GenerateBuilderVillageGameObjectGlobalId(go);
                if (go.ClassId == 7)
                {
                    var b = (Builder_Building)go;
                    var bd = b.GetBuildingData;
                    if (bd.IsWorker2Building())
                        this.Level.BuilderVillageWorkerManager.IncreaseWorkerCount();
                }
            }
            this.GameObjects[go.ClassId].Add(go);
        }

        public List<List<GameObject>> GetAllGameObjects() => this.GameObjects;

        public ComponentManager GetComponentManager() => this.ComponentManager;

        //public ObstacleManager GetObstacleManager() => m_vObstacleManager;

        public GameObject GetGameObjectByID(int id)
        {
            var classId = GlobalID.GetType(id) - 500;
            if (this.GameObjects.Capacity < classId)
                return null;
            return this.GameObjects[classId].Find(g => g.GlobalId == id);
        }

        public GameObject GetBuilderVillageGameObjectByID(int id)
        {
            var classId = GlobalID.GetType(id) - 493;
            if (this.GameObjects.Capacity < classId)
                return null;
            return this.GameObjects[classId].Find(g => g.GlobalId == id);
        }


        public List<GameObject> GetGameObjects(int id) => this.GameObjects[id];

        public JObject JSON
        {
            get
            {
                #region Get
                JArray JBuildings = new JArray();
                int c = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[0]))
                {
                    Building b = (Building)go;
                    JObject j = new JObject { { "data", b.GetBuildingData.GetGlobalID() }, { "id", 500000000 + c } };
                    b.Save(j);
                    JBuildings.Add(j);
                    c++;
                }

                JArray JObstacles = new JArray();
                int o = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[3]))
                {
                    Obstacle d = (Obstacle)go;
                    JObject j = new JObject { { "data", d.GetObstacleData().GetGlobalID() }, { "id", 503000000 + o } };
                    d.Save(j);
                    JObstacles.Add(j);
                    o++;
                }

                JArray JTraps = new JArray();
                int u = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[4]))
                {
                    Trap t = (Trap)go;
                    JObject j = new JObject { { "data", t.GetTrapData.GetGlobalID() }, { "id", 504000000 + u } };
                    t.Save(j);
                    JTraps.Add(j);
                    u++;
                }

                JArray JDecos = new JArray();
                int e = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[6]))
                {
                    Deco d = (Deco)go;
                    JObject j = new JObject { { "data", d.GetDecoData().GetGlobalID() }, { "id", 506000000 + e } };
                    d.Save(j);
                    JDecos.Add(j);
                    e++;
                }


                JArray JObject = new JArray();
                int jO = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[8]))
                {
                    Village_Object d = (Village_Object)go;
                    JObject j = new JObject { { "data", d.GetVillageObjectsData.GetGlobalID() }, { "id", 508000000 + jO } };
                    d.Save(j);
                    JObject.Add(j);
                    jO++;
                }

                JArray JBuildings2 = new JArray();
                int c2 = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[7]))
                {
                    Builder_Building b = (Builder_Building)go;
                    JObject j = new JObject { { "data", b.GetBuildingData.GetGlobalID() }, { "id", 500000000 + c2 } };
                    b.Save(j);
                    JBuildings2.Add(j);
                    c2++;
                }

                JArray JObstacles2 = new JArray();
                int o2 = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[10]))
                {
                    Builder_Obstacle d = (Builder_Obstacle)go;
                    JObject j = new JObject { { "data", d.GetObstacleData.GetGlobalID() }, { "id", 503000000 + o2 } };
                    d.Save(j);
                    JObstacles2.Add(j);
                    o2++;
                }

                JArray JTraps2 = new JArray();
                int u2 = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[11]))
                {
                    Builder_Trap t = (Builder_Trap)go;
                    JObject j = new JObject { { "data", t.GetTrapData.GetGlobalID() }, { "id", 504000000 + u2 } };
                    t.Save(j);
                    JTraps2.Add(j);
                    u2++;
                }


                JArray JDecos2 = new JArray();
                int e2 = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[13]))
                {
                    Builder_Deco d = (Builder_Deco)go;
                    JObject j = new JObject { { "data", d.GetDecoData.GetGlobalID() }, { "id", 506000000 + e2 } };
                    d.Save(j);
                    JDecos2.Add(j);
                    e2++;
                }

                JArray JObject2 = new JArray();
                int jO2 = 0;
                foreach (GameObject go in new List<GameObject>(this.GameObjects[15]))
                {
                    Builder_Village_Object d = (Builder_Village_Object)go;
                    JObject j = new JObject { { "data", d.GetVillageObjectsData.GetGlobalID() }, { "id", 508000000 + jO2 } };
                    d.Save(j);
                    JObject2.Add(j);
                    jO2++;
                }

                Player pl = this.Level.Avatar;
                /*
                var unitSlot = new JArray
                {new JObject
                    {
                        {"id", 4000001},
                        {"cnt", 1},
                    },
                    new JObject
                    {
                        {"id", 4000001},
                        {"cnt", 26},
                        {"t", true }
                    },

                };
                var unit = new JObject
                {
                    {"t", TimeUtils.ToTick(TimeSpan.FromSeconds(0.5)) },
                    {"slots", unitSlot }

                };
                */
                var jsonData = new JObject
                {
                    {"exp_ver", 1},
                    {"android_client", true},
                    {"active_layout", 0 },
                    {"act_l2", 0},
                    {"war_layout", 0 },
                    {"layout_state", new JArray {0, 0, 0, 0, 0, 0, 0, 0}},
                    {"layout_state2", new JArray {0, 0, 0, 0, 0, 0, 0, 0}},
                    {"layout_cooldown", new JArray {0, 0, 0, 0, 0, 0, 0, 0}},
                    {"buildings", JBuildings},
                    {"obstacles", JObstacles},
                    {"traps", JTraps},
                    {"decos", JDecos},
                    {"vobjs", JObject},
                    {"units",new JArray { "unit_prod" } },
                    {"spells",new JArray { "unit_prod" } },
                    {"buildings2", JBuildings2},
                    {"obstacles2", JObstacles2},
                    {"traps2", JTraps2},
                    {"decos2", JDecos2},
                    {"vobjs2", JObject2},
                    {"offer", new JObject() },                  
                    {"last_league_rank", 12 },
                    {"last_alliance_level", 1 },
                    {"last_league_shuffle", 0 },
                    {"last_season_seen", 12 },
                    {"last_news_seen", 999999999 },
                    {"war_tutorials_seen", 0 },
                    {"war_base", true },
                    {"arr_war_base", false},
                    {"account_flags", 15},
                    {"bool_layout_edit_shown_erase", true},

                };

                return jsonData;
#endregion
            }
            set 
            {
                var jsonBuildings = (JArray) value["buildings"];
                foreach (JObject jsonBuilding in jsonBuildings)
                {
                    var bd =
                        CSV.Tables.Get(Gamefile.Buildings)
                            .GetDataWithID(jsonBuilding["data"].ToObject<int>()) as Buildings;
                    var b = new Building(bd, this.Level);
                    AddGameObject(b);
                    b.Load(jsonBuilding);
                }

                var jsonObstacles = (JArray)value["obstacles"];
                foreach (JObject jsonObstacle in jsonObstacles)
                {
                    var dd = CSV.Tables.Get(Gamefile.Obstacles).GetDataWithID(jsonObstacle["data"].ToObject<int>()) as Obstacles;
                    var d = new Obstacle(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonObstacle);
                }

                var jsonTraps = (JArray) value["traps"];
                foreach (JObject jsonTrap in jsonTraps)
                {
                    var td = CSV.Tables.Get(Gamefile.Traps).GetDataWithID(jsonTrap["data"].ToObject<int>()) as Traps;
                    var t = new Trap(td, this.Level);
                    AddGameObject(t);
                    t.Load(jsonTrap);
                }

                var jsonDecos = (JArray) value["decos"];
                foreach (JObject jsonDeco in jsonDecos)
                {
                    var dd = CSV.Tables.GetWithGlobalID(jsonDeco["data"].ToObject<int>()) as Decos;
                    var d = new Deco(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonDeco);
                }

                var jsonObjects = (JArray)value["vobjs"];
                foreach (JObject jsonObject in jsonObjects)
                {
                    var dd = CSV.Tables.GetWithGlobalID(jsonObject["data"].ToObject<int>()) as Village_Objects;
                    var d = new Village_Object(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonObject);
                }

                var jsonBuildings2 = (JArray)value["buildings2"];
                foreach (JObject jsonBuilding2 in jsonBuildings2)
                {
                    var bd =
                        CSV.Tables.Get(Gamefile.Buildings)
                            .GetDataWithID(jsonBuilding2["data"].ToObject<int>()) as Buildings;
                    var b = new Builder_Building(bd, this.Level);
                    AddGameObject(b);
                    b.Load(jsonBuilding2);
                }

                var jsonObstacles2 = (JArray)value["obstacles2"];
                foreach (JObject jsonObstacle2 in jsonObstacles2)
                {
                    var dd = CSV.Tables.Get(Gamefile.Obstacles).GetDataWithID(jsonObstacle2["data"].ToObject<int>()) as Obstacles;
                    var d = new Builder_Obstacle(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonObstacle2);
                }

                var jsonTraps2 = (JArray)value["traps2"];
                foreach (JObject jsonTrap2 in jsonTraps2)
                {
                    var td = CSV.Tables.Get(Gamefile.Traps).GetDataWithID(jsonTrap2["data"].ToObject<int>()) as Traps;
                    var t = new Builder_Trap(td, this.Level);
                    AddGameObject(t);
                    t.Load(jsonTrap2);
                }

                var jsonDecos2 = (JArray)value["decos2"];
                foreach (JObject jsonDeco2 in jsonDecos2)
                {
                    var dd = CSV.Tables.GetWithGlobalID(jsonDeco2["data"].ToObject<int>()) as Decos;
                    var d = new Builder_Deco(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonDeco2);
                }

                var jsonObjects2 = (JArray)value["vobjs2"];
                foreach (JObject jsonObject2 in jsonObjects2)
                {
                    var dd = CSV.Tables.GetWithGlobalID(jsonObject2["data"].ToObject<int>()) as Village_Objects;
                    var d = new Builder_Village_Object(dd, this.Level);
                    AddGameObject(d);
                    d.Load(jsonObject2);
                }
                /*
    
                m_vObstacleManager.Load(jsonObject); */
            }
        }

        public void RemoveGameObject(GameObject go)
        {
            this.GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorkerBuilding())
                {
                   this.Level.VillageWorkerManager.DecreaseWorkerCount();
                }
            }
            if (go.ClassId == 7)
            {
                var b = (Builder_Building)go;
                var bd = b.GetBuildingData;
                if (bd.IsWorker2Building())
                {
                    this.Level.BuilderVillageWorkerManager.DecreaseWorkerCount();
                }
            }
            RemoveGameObjectReferences(go);
        }

        public void RemoveGameObjectReferences(GameObject go)
        {
            this.ComponentManager.RemoveGameObjectReferences(go);
        }

        public void Tick()
        {
            this.ComponentManager.Tick();
            foreach (var l in this.GameObjects.ToList())
            {
                foreach (var go in l.ToList())
                    go.Tick();
            }
            foreach (var g in new List<GameObject>(this.GameObjectRemoveList))
            {
                RemoveGameObjectTotally(g);
                this.GameObjectRemoveList.Remove(g);
            }
        }

        internal int GenerateGameObjectGlobalId(GameObject go)
        {
            var index = this.GameObjectsIndex[go.ClassId];
            this.GameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 500, index);
        }
        internal int GenerateBuilderVillageGameObjectGlobalId(GameObject go)
        {
            var index = this.GameObjectsIndex[go.ClassId];
            this.GameObjectsIndex[go.ClassId]++;
            return GlobalID.CreateGlobalID(go.ClassId + 493, index);
        }

        internal void RemoveGameObjectTotally(GameObject go)
        {
            this.GameObjects[go.ClassId].Remove(go);
            if (go.ClassId == 0)
            {
                var b = (Building) go;
                var bd = b.GetBuildingData;
                if (bd.IsWorkerBuilding())
                    this.Level.VillageWorkerManager.DecreaseWorkerCount();
            }
            if (go.ClassId == 7)
            {
                var b = (Builder_Building)go;
                var bd = b.GetBuildingData;
                if (bd.IsWorker2Building())
                    this.Level.BuilderVillageWorkerManager.DecreaseWorkerCount();
            }
            RemoveGameObjectReferences(go);
        }
    }
}
