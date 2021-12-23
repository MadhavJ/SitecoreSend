using Newtonsoft.Json;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Send.Util.Services
{
    public partial class MooSendSubmitAction : SubmitActionBase<string>
    {
        private readonly Uri baseAddress = new Uri("https://api.moosend.com/v3/");
        private readonly string memberListId = "987c9c1c-7231-4a7e-888c-19ba1e109a14"; // Update with member list ID from your Sitecore Send instance
        private readonly string mooSendApiKey = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXX"; //Update with Send unique API key from your Sitecore Send instance

        public MooSendSubmitAction(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

       
        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, "formSubmitContext");
            var mooSendData = GetMooSendJson(formSubmitContext);
            Task<Task> executePost = Task.Run(async () => await ExecuteCall(JsonConvert.SerializeObject(mooSendData)));
            return true;
        }

        private async Task<Task> ExecuteCall(string data)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent(data, Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync($"subscribers/{memberListId}/subscribe.json?apiKey={mooSendApiKey}", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseData))
                        {
                            Console.WriteLine("Not able to submit to moosend");
                        }
                        return Task.CompletedTask;
                    }
                }
            }
        }

        private MooSendData GetMooSendJson(FormSubmitContext formSubmitContext)
        {
            IViewModel firstname = formSubmitContext.Fields.FirstOrDefault(x => x.Name.ToLower().Equals("firstname"));
            IViewModel lastname = formSubmitContext.Fields.FirstOrDefault(x => x.Name.ToLower().Equals("lastname"));
            IViewModel email = formSubmitContext.Fields.FirstOrDefault(x => x.Name.ToLower().Equals("email"));
            IViewModel interest = formSubmitContext.Fields.FirstOrDefault(x => x.Name.ToLower().Equals("interest"));
            IViewModel mobileNumber = formSubmitContext.Fields.FirstOrDefault(x => x.Name.ToLower().Equals("mobile"));


            string name = GetFieldValue(firstname) + " " + GetFieldValue(lastname);
            string emailAddress = GetFieldValue(email);
            string interestedSport = GetFieldValue(interest);
            string contactNumber = GetFieldValue(mobileNumber);

            var model = new MooSendData
            {
                Name = name,
                Email = emailAddress,
                Mobile = contactNumber,
                CustomFields = new List<string>() { "interest=" + interestedSport }
            };

            //Log.Info("name: " + name, this);
            //Log.Info("email: " + emailAddress, this);

            return model;
        }

        private string GetFieldValue(IViewModel field, bool single = false)
        {
            try
            {
                return field is ListViewModel model
                    ? single ? model?.Value?[0] : string.Join(",", model.Value)
                    : field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Forms GetValue: " + ex.Message.ToString(), "GetValue");
                return null;
            }
        }
    }
}