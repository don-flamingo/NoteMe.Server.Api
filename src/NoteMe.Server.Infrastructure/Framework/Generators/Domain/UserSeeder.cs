using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NoteMe.Server.Core.Models;

namespace NoteMe.Server.Infrastructure.Framework.Generators.Domain
{
    public class UserSeeder : IDataSeeder<User>
    {
        public static Guid TestId = new Guid("e6ca1655-b16c-4ef6-afa0-2d4a71e8912c");
        public static string TestEmail = "test@gmail.com";
        public static string TestPassword = "admin123";
        public static string TestHash = "pQB04yfF5cJBcjhTU4oj4dOAiugMgxp/0D0Ia8eZP/va+XYlbpokzw==";
        public static string TestSalt = "QxcOcrocA+Ah4nI7KYx88kT5hCdJFLMWYljedF6FXqpkOzzmqk8Xsg==";
        
        public static Guid TestNote1Id = new Guid("ec626ae0-7dca-4e71-95f5-12ea3a07db65");
        public static Guid TestNote2Id = new Guid("390a55b8-8ddb-4061-b041-9ee88a940d18");
        public static Guid TestNote3Id = new Guid("24688570-ae5c-4828-a1c5-2e82653c60c6");
        
        public static Guid TestNote3Hist1Id = new Guid("ac163b02-526d-42c2-8142-247512c13e19");
        public static Guid TestNote3Hist2Id = new Guid("f89ea9fd-feaf-4423-9833-5aee6e485ef0");
        public static Guid TestNote3Hist3Id = new Guid("b03b7813-5ea1-4fbf-8ea8-b82428f2d77d");

        public static decimal WarsawLat = 52.229675m;
        public static decimal WarsawLng = 21.012230m;
            
        public ICollection<User> GetDataToSeed()
        {
            return new List<User>
            {
                new User
                {
                    Id = TestId,
                    Name = "Johhny",
                    CreatedAt = DateTime.UtcNow,
                    Email = TestEmail,
                    Hash = TestHash,
                    Salt = TestSalt,
                    Notes = new List<Note>
                    {
                        new Note
                        {
                            Id = TestNote1Id,
                            Name = "What is Lorem Ipsum?",
                            Content =
                                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Latitude = WarsawLat,
                            Longitude = WarsawLng,
                            CreatedAt = DateTime.UtcNow,
                            UserId = TestId,
                        },
                        new Note
                        {
                            Id = TestNote2Id,
                            Name = "Why do we use it?",
                            Content =
                                "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Latitude = WarsawLat,
                            Longitude = WarsawLng,
                            CreatedAt = DateTime.UtcNow,
                            UserId = TestId,
                        },
                        new Note
                        {
                            Id = TestNote3Id,
                            Name = "Where can I get some?",
                            Content =
                                "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Latitude = WarsawLat,
                            Longitude = WarsawLng,
                            CreatedAt = DateTime.UtcNow,
                            UserId = TestId,
                            OldNotes = new List<Note>
                            {
                                new Note
                                {
                                    Id = TestNote3Hist1Id,
                                    Name = "Where can I get some? - almost",
                                    Content =
                                        "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. ",
                                    Latitude = WarsawLat,
                                    Longitude = WarsawLng,
                                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(1),
                                    UserId = TestId,
                                    ActualNoteId = TestNote3Id,
                                },
                                new Note
                                {
                                    Id = TestNote3Hist2Id,
                                    Name = "Where can I get some? - middle",
                                    Content =
                                        "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet.",
                                    Latitude = WarsawLat,
                                    Longitude = WarsawLng,
                                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(2),
                                    UserId = TestId,
                                    ActualNoteId = TestNote3Id,
                                },
                                new Note
                                {
                                    Id = TestNote3Hist3Id,
                                    Name = "Where can I get some? - begin",
                                    Content =
                                        "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                                    Latitude = WarsawLat,
                                    Longitude = WarsawLng,
                                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(3),
                                    UserId = TestId,
                                    ActualNoteId = TestNote3Id,
                                }
                            }
                        },
                    }
                }
            };
        }
    }
}