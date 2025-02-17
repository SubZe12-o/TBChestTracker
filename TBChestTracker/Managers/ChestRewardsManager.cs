﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TBChestTracker.Managers;

namespace TBChestTracker
{
    public class ChestRewardsManager : IDisposable
    {
        public List<ChestReward> ChestRewardsList = new List<ChestReward>();
        public static ChestRewardsManager Instance { get; private set; }
        public ChestRewardsManager() 
        { 
            if(Instance == null)
            {
                Instance = this;
            }
        }

        public void Add(ChestReward chestReward)
        {
            ChestRewardsList.Add(chestReward);
        }
        public void Add(string chesttype, int level, string reward)
        {
            ChestRewardsList.Add(new ChestReward(chesttype, level, reward));
        }

        public bool Load()
        {
            var root = $"{SettingsManager.Instance.Settings.GeneralSettings.ClanRootFolder}";
            var clanFolder = $"{root}{ClanManager.Instance.ClanDatabaseManager.ClanDatabase.ClanFolderPath}";
            var databaseFolder = $"{clanFolder}{ClanManager.Instance.ClanDatabaseManager.ClanDatabase.ClanDatabaseFolder}";
            var file = $@"{databaseFolder}\ChestRewards.db";
            var _chestrewardslist =  new List<ChestReward>();
            if(JsonHelper.TryLoad(file, out _chestrewardslist))
            {
                ChestRewardsList = _chestrewardslist;
                return true;
            }
            else
            {
                return false;
            }
            /*
            using(StreamReader sr = File.OpenText(file))
            {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                ChestRewardsList = (List<ChestReward>)serializer.Deserialize(sr, typeof(List<ChestReward>));
                sr.Close();
                serializer = null;
            }
            */
        }
        public void Save()
        {
            var root = $"{SettingsManager.Instance.Settings.GeneralSettings.ClanRootFolder}";
            var clanfolder = $"{root}{ClanManager.Instance.ClanDatabaseManager.ClanDatabase.ClanFolderPath}";

            var file = $@"{clanfolder}{ClanManager.Instance.ClanDatabaseManager.ClanDatabase.ClanDatabaseFolder}\ChestRewards.db";
            using(StreamWriter sw = File.CreateText(file))
            {
                var serializer = new JsonSerializer();  
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(sw, ChestRewardsList);
                sw.Close();
                serializer = null;
            }
        }
        public void Dispose()
        {
            ChestRewardsList.Clear();
        }
    }
}
