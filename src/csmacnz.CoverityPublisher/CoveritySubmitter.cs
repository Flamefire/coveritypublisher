﻿using System;
using System.IO;
using System.Net.Http;

namespace csmacnz.CoverityPublisher
{
    public class CoveritySubmitter
    {
        public static ActionResult Submit(PublishPayload payload)
        {
            using (var fs = new FileStream(payload.FileName, FileMode.Open, FileAccess.Read))
            {
                using (var form = new MultipartFormDataContent
                {
                    {new StringContent(payload.Token), "token"},
                    {new StringContent(payload.Email), "email"},
                    {new StreamContent(fs), "file", payload.FileName},
                    {new StringContent(payload.Version), "version"},
                    {new StringContent(payload.Description), "description"}
                })
                {

                    var url = string.Format("https://scan.coverity.com/builds?project={0}", payload.RepositoryName);

                    ActionResult results = new ActionResult
                    {
                        Successful = true,
                    };
                    if (payload.SubmitToCoverity)
                    {
                        try
                        {
                            var response = Client.Post(url, form);
                            if (response.IsSuccessStatusCode)
                            {
                                results.Message = "Request Submitted Successfully";
                            }
                            else
                            {
                                results.Successful = false;
                                results.Message = "There was an error submitting your report: \n" +
                                                  response.ReasonPhrase;
                            }
                        }
                        catch (AggregateException exception)
                        {
                            var ex = exception.InnerException;
                            results.Successful = false;
                            results.Message = "There was an error submitting your report: \n" + ex;
                        }
                    }
                    else
                    {
                        results.Message = "Dry run Successful";
                    }
                    return results;
                }
            }
        }
    }
}