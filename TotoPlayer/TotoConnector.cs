using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotoLogic;

namespace TotoPlayer
{
    public class TotoConnector : PropertyChangedBase
    {
        public string Token { get; set; }
        private string poolUrl = "https://api.toto15.top/v1/pool/{0}/all?real=true";

        private string log = "Initialized...";
        public string Log
        {
            get => log;
            set
            {
                log = value;
                OnPropertyChanged();
            }
        }

        private void PrintLog(string value) => Log += "\n" + value;

        public TotoPool GetTotoPool(string ID)
        {
            TotoPool totoPool = new TotoPool() { ID = ID };
            try
            {
                dynamic resp = GetRestResponse(string.Format(poolUrl, ID));
                if (resp.error != null) { PrintLog(resp.message.ToString()); return totoPool; }

                for (int i = 0; i < TotoConstants.MatchesCount; i++)
                {
                    totoPool.Matches[i] = new TotoMatch
                    {
                        LeagueName = resp.matches[i].league.name,
                        HostsName = resp.matches[i].hosts.name,
                        VisitorsName = resp.matches[i].visitors.name,
                        IsSpecial = resp.matches[i].special,
                        HandicapBasis = resp.matches[i].handicapBasis,
                        TotalBasis = resp.matches[i].totalBasis,
                        StartTime = TimestampConverter.FromMilliseconds((double)resp.matches[i].start)
                    };
                    totoPool.Matches[i].Bets[0] = new TotoBet((double)resp.probs[i].hostsOver, (double)resp.poolDistribution[i].hostsOver);
                    totoPool.Matches[i].Bets[1] = new TotoBet((double)resp.probs[i].visitorsOver, (double)resp.poolDistribution[i].visitorsOver);
                    totoPool.Matches[i].Bets[2] = new TotoBet((double)resp.probs[i].hostsUnder, (double)resp.poolDistribution[i].hostsUnder);
                    totoPool.Matches[i].Bets[3] = new TotoBet((double)resp.probs[i].visitorsUnder, (double)resp.poolDistribution[i].visitorsUnder);
                }
                totoPool.InitializeBetsList();
            } catch(Exception ex) { PrintLog(ex.Message); }
            return totoPool;
        }
        /*
        public bool SendCoupon(TotoPool totoPool)
        {
            dynamic obj = new
            {
                pool = totoPool.ID,
                selections = totoPool.Matches.Select(e => e.Bets.Select(bet => bet.Selected ? 1 : 0).ToArray()).ToArray(),
                multiplier = totoPool.Multiplier,
                guarantee = 15,
                source = "full"
            };
        }*/

        private JToken GetRestResponse(string url)
        {
            var request = new RestRequest();
            request.AddHeader("lang", "ru");

            var response = new RestClient(url).Execute(request);
            return JToken.Parse(response.Content);
        }


        private JToken GetRestResponse(RestClient client, object obj)
        {
            var response = client.Execute(GetRestRequest(obj));
            return JToken.Parse(response.Content);
        }
        private RestClient GetRestClient(string requestUrl)
        {
            return new RestClient(requestUrl);
        }

        private RestRequest GetRestRequest(object obj)
        {
        //    var jsonObj = JsonConvert.SerializeObject(obj);
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", Token);
            request.AddBody(obj);
            //request.AddHeader(ApiBfxPayload, payload);
            //request.AddHeader(ApiBfxSig, GetHexHashSignature(payload));
            return request;
        }
    }

    public static class TimestampConverter
    {
        static DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromSeconds(double seconds) => dtDateTime.AddSeconds(seconds);
        public static DateTime FromMilliseconds(double seconds) => dtDateTime.AddMilliseconds(seconds);
    }
}
