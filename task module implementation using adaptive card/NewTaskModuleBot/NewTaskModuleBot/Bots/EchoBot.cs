// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewTaskModuleBot.Bots
{
    public class EchoBot : TeamsActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            dynamic cardPostBack = turnContext.Activity.Value;
            var str = turnContext.Activity.Text;
            if (str == "hi" || str == "hello")
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(Schoolcard()), cancellationToken);
            }
            //else if (str == "login")
            //{
            //    await turnContext.SendActivityAsync(MessageFactory.Attachment(Logcard()), cancellationToken);
            //}
            else if (cardPostBack["Id"].ToString() == "login")
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(Logincard()), cancellationToken);
            }
        }
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(taskModuleRequest);
            await turnContext.SendActivityAsync(json);
            var obj = JObject.Parse(json);
            var name = (string)obj["data"]["name"];
            var uname = (string)obj["data"]["uname"];

            //var uname = (string)obj["data"]["name"];
            return new TaskModuleResponse
            {
                Task = new TaskModuleContinueResponse
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Card = CreateAdaptiveCardAttachment(name, uname),
                        Height = 200,
                        Width = 400,
                        Title = "Adaptive Card: Inputs",
                    },
                }
            };
        }
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleSubmitAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("OnTeamsTaskModuleSubmitAsync Value: " + JsonConvert.SerializeObject(taskModuleRequest));
            await turnContext.SendActivityAsync(reply);

            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse()
                {
                    Value = "Thanks!",
                },
            };
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
        public static Attachment Schoolcard()
        {
            AdaptiveCard card = new AdaptiveCard("1.2")
            {
                Body = new List<AdaptiveElement>()
                {
                    
                        new AdaptiveTextBlock
                        {
                            Text="Enter to school",
                            Size=AdaptiveTextSize.Large,
                            HorizontalAlignment = AdaptiveHorizontalAlignment.Center,

                        },
                    
                    
                    new AdaptiveActionSet()
                    {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveShowCardAction
                            {

                                Title = "Enter Here",
                                Style = "destructive",
                                Card =new AdaptiveCard("1.2")
                                {
                                    Body=new List<AdaptiveElement>()
                                    {
                                        new AdaptiveColumnSet
                                       {
                                            Columns=new List<AdaptiveColumn>()
                                               {
                                                 new AdaptiveColumn
                                                  {
                                                   Width=AdaptiveColumnWidth.Auto,
                                                   VerticalContentAlignment=AdaptiveVerticalContentAlignment.Center,
                                                   Style=AdaptiveContainerStyle.Emphasis,
                                                    Items=new List<AdaptiveElement>()
                                                     {
                                                       new AdaptiveTextBlock
                                                        {
                                                          Text="Enter Studentname",
                                                          Size=AdaptiveTextSize.Small,

                                                         }
                                                      }
                             },
                            new AdaptiveColumn
                            {
                                VerticalContentAlignment=AdaptiveVerticalContentAlignment.Center,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="Username",
                                        Id="uname"
                                    }
                                }
                            },
                        },

                    },

                    

                                        new AdaptiveColumnSet
                                        {
                                            Columns=new List<AdaptiveColumn>()
                                            {
                                                new AdaptiveColumn
                                                {
                                                    Items=new List<AdaptiveElement>()
                                                    {
                                                        new AdaptiveActionSet
                                                        {
                                                            Actions=new List<AdaptiveAction>()
                                                            {
                                                                new AdaptiveSubmitAction
                                                                {
                                                                    Title = "Submit",
                                                                    Style = "positive",
                                                                    Type=AdaptiveSubmitAction.TypeName,
                                                                    Data = new Dictionary<string, object>(){ {"msteams",new Dictionary<string,string>(){ {"type","task/fetch"},{"value","{\"Id\":\"name\"}"} }},{"data","submit"} }
                                                                }
                                                            }
                                                        }
                                                    }
                                                },
                                                new AdaptiveColumn
                                                {
                                                    Items=new List<AdaptiveElement>()
                                                    {
                                                        new AdaptiveActionSet
                                                        {
                                                            Actions=new List<AdaptiveAction>()
                                                            {
                                                                new AdaptiveSubmitAction
                                                                {
                                                                    Title = "login",
                                                                    Style = "positive",
                                                                    Type=AdaptiveSubmitAction.TypeName,
                                                                    Data = new Dictionary<string, object>(){ {"msteams",new Dictionary<string,string>(){ {"type","messageBack"},{"displayText", "login"},{"value","{\"Id\":\"login\"}"} }} }

                                                                },
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                    
                }
            };

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }
        public static Attachment Logincard()
        {
            AdaptiveCard card = new AdaptiveCard("1.2")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="Enter Details" ,
                        Size=AdaptiveTextSize.Large,
                        HorizontalAlignment=AdaptiveHorizontalAlignment.Center,
                        Color=AdaptiveTextColor.Good
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Width=AdaptiveColumnWidth.Auto,
                                VerticalContentAlignment=AdaptiveVerticalContentAlignment.Center,
                                Style=AdaptiveContainerStyle.Emphasis,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Enter Username",
                                        Size=AdaptiveTextSize.Small,

                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                VerticalContentAlignment=AdaptiveVerticalContentAlignment.Center,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="Username",
                                        Id="uname"
                                    }
                                }
                            },
                        },

                    },

                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Width=AdaptiveColumnWidth.Auto,
                                VerticalContentAlignment=AdaptiveVerticalContentAlignment.Center,
                                Style=AdaptiveContainerStyle.Emphasis,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Enter Password",
                                        Size=AdaptiveTextSize.Small,

                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                VerticalContentAlignment=AdaptiveVerticalContentAlignment.Center,
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="Password",
                                        Id="pwd"
                                    }
                                }
                            },
                        },

                    },

                    new AdaptiveActionSet
                    {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = "Login",
                                Style = "positive",
                                Type=AdaptiveSubmitAction.TypeName,
                                Data= new Dictionary<string, object>()
                                {
                                    {
                                    "msteams",new Dictionary<string,string>()
                                        {
                                            {
                                            "type","task/fetch"
                                            },
                                            {
                                            "value","{\"Id\":\"uname\"}"
                                            //"value",json
                                            }
                                        }
                                    },
                                    {
                                    "data","submit"
                                    }
                                }
                            },
                        }
                    }
                }
            };

            Attachment attach = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attach;
        }
        public Attachment CreateAdaptiveCardAttachment(string name, string uname)
        {
            if (name == null && uname != null)
            {
                AdaptiveCard card = new AdaptiveCard("1.0")
                {
                    Body = new List<AdaptiveElement>()
                    {
                        new AdaptiveTextBlock
                        {
                            Text="Hello "+uname
                        }
                    }
                };
                Attachment at = new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                };
                return at;
            }
            else
            {
                AdaptiveCard card1 = new AdaptiveCard("1.0")
                {
                    Body = new List<AdaptiveElement>()
                    {
                        new AdaptiveTextBlock
                        {
                            Text="your name is: "+name
                        }
                    }
                };
                Attachment at1 = new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card1
                };
                return at1;
            }


        }
    }
}
