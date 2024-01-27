using Microsoft.AspNetCore.Mvc.Testing;
using Notepad.Api.Notes.Models.Requests;
using Notepad.Api.Notes.Models.Responses;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Notepad.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Notepad.Core.Models;

namespace Notepad.Api.Integration.Test.Notepad
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<MyDbContext>));

                services.Remove(dbContextDescriptor!);

                // put some test db conn string
                services.AddDbContext<MyDbContext>((container, options) =>
                {
                    options.UseNpgsql("test db connection string");
                });
            });

            builder.UseEnvironment("Development");
        }
    }

    [TestFixture]
    [Ignore("Run on test db, setup connection string and jwt token")]
    public class NotepadControllerTests
    {
        private CustomWebApplicationFactory<Program> _factory;
        private HttpClient _client;

        private const string jwtToken = "jwtToken of some user, might be obtained from development.jwt";


        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        [Test]
        public async Task CreateNote_ReturnsOkResponse()
        {
            var request = new CreateNoteRequest {
                Content = "some random content +48234678923"
            };
            var response = await _client.PostAsJsonAsync("/notepad", request);

            Assert.That(response.IsSuccessStatusCode, Is.True);

            var noteResponse = await response.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse, Is.Not.Null);
            Assert.That(noteResponse.Content, Is.EqualTo(request.Content));
            var expected = new List<string> { "PHONE" };
            CollectionAssert.AreEquivalent(expected, noteResponse.Tags);

            // cleanup
            await _client.DeleteAsync($"/notepad?noteId={noteResponse.NoteId}");
        }

        [Test]
        public async Task CreateUpdateNote_ModifiesNote()
        {
            #region create note
            var request = new CreateNoteRequest
            {
                Content = "some random content +48234678923"
            };
            var response = await _client.PostAsJsonAsync("/notepad", request);

            Assert.That(response.IsSuccessStatusCode, Is.True);

            var noteResponse = await response.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse, Is.Not.Null);

            var noteId = noteResponse.NoteId;
            #endregion
            #region update note
            var request2 = new UpdateNoteRequest
            {
                Content = "some random content s@gmail.com",
                NoteId = noteId
            };
            var response2 = await _client.PatchAsJsonAsync("/notepad", request2);

            Assert.That(response2.IsSuccessStatusCode, Is.True);

            var noteResponse2 = await response2.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse2, Is.Not.Null);
            Assert.That(noteResponse2.Content, Is.EqualTo(request2.Content));
            var expected = new List<string> { "EMAIL" };
            CollectionAssert.AreEquivalent(expected, noteResponse2.Tags);
            Assert.That(noteResponse2.NoteId, Is.EqualTo(noteResponse.NoteId));
            #endregion
            // cleanup
            await _client.DeleteAsync($"/notepad?noteId={noteId}");
        }

        [Test]
        public async Task DeleteNote_DeletesCorrectNotes()
        {
            #region create note
            var request = new CreateNoteRequest
            {
                Content = "some random content +48234678923"
            };
            var response = await _client.PostAsJsonAsync("/notepad", request);

            Assert.That(response.IsSuccessStatusCode, Is.True);

            var noteResponse = await response.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse, Is.Not.Null);

            var noteId = noteResponse.NoteId;
            #endregion
            #region check it exists
            // check it exists
            var response2 = await _client.GetAsync("/notepad?tags=PHONE");
            Assert.That(response2.IsSuccessStatusCode, Is.True);

            var noteResponses = await response2.Content.ReadFromJsonAsync<IEnumerable<NoteResponse>>();
            Assert.That(noteResponses, Is.Not.Null);

            Assert.That(noteResponses.Count(), Is.EqualTo(1));
            #endregion

            #region delete note
            var response3 = await _client.DeleteAsync($"/notepad?noteId={noteId}");
            Assert.That(response3, Is.Not.Null);
            #endregion
            #region check it does not exists
            var response4 = await _client.GetAsync("/notepad?tags=PHONE");
            Assert.That(response4.IsSuccessStatusCode, Is.True);

            var noteResponses2 = await response4.Content.ReadFromJsonAsync<IEnumerable<NoteResponse>>();
            Assert.That(noteResponses2, Is.Not.Null);

            Assert.That(noteResponses2.Count(), Is.EqualTo(0));
            #endregion
        }


        [Test]
        public async Task GetNote_WitTags_FiltersCorrectly()
        {
            #region create phone note
            var request = new CreateNoteRequest
            {
                Content = "some random content +48234678923"
            };
            var response = await _client.PostAsJsonAsync("/notepad", request);

            Assert.That(response.IsSuccessStatusCode, Is.True);

            var noteResponse = await response.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse, Is.Not.Null);

            var noteId = noteResponse.NoteId;
            #endregion
            #region create mail note
            var request2 = new CreateNoteRequest
            {
                Content = "some random content s@gmail.com"
            };
            var response2 = await _client.PostAsJsonAsync("/notepad", request2);

            Assert.That(response2.IsSuccessStatusCode, Is.True);

            var noteResponse2 = await response2.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse2, Is.Not.Null);

            var noteId2 = noteResponse2.NoteId;
            #endregion
            #region create no tag note
            var request3 = new CreateNoteRequest
            {
                Content = "some random content"
            };
            var response3 = await _client.PostAsJsonAsync("/notepad", request3);

            Assert.That(response3.IsSuccessStatusCode, Is.True);

            var noteResponse3 = await response3.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse3, Is.Not.Null);

            var noteId3 = noteResponse3.NoteId;
            #endregion
            #region create phone mail note
            var request4 = new CreateNoteRequest
            {
                Content = "some random content s@gmail.com +48234678923"
            };
            var response4 = await _client.PostAsJsonAsync("/notepad", request4);

            Assert.That(response4.IsSuccessStatusCode, Is.True);

            var noteResponse4 = await response4.Content.ReadFromJsonAsync<NoteResponse>();
            Assert.That(noteResponse4, Is.Not.Null);

            var noteId4 = noteResponse4.NoteId;
            #endregion

            #region get all notes
            var response5 = await _client.GetAsync("/notepad");
            Assert.That(response5.IsSuccessStatusCode, Is.True);

            var noteResponses1 = await response5.Content.ReadFromJsonAsync<IEnumerable<NoteResponse>>();
            Assert.That(noteResponses1, Is.Not.Null);

            Assert.That(noteResponses1.Count(), Is.EqualTo(4));
            #endregion

            #region get phone notes
            var response6 = await _client.GetAsync("/notepad?tags=PHONE");
            Assert.That(response6.IsSuccessStatusCode, Is.True);

            var noteResponses2 = await response6.Content.ReadFromJsonAsync<IEnumerable<NoteResponse>>();
            Assert.That(noteResponses2, Is.Not.Null);

            Assert.That(noteResponses2.Count(), Is.EqualTo(2));
            #endregion

            #region get mail notes
            var response7 = await _client.GetAsync("/notepad?tags=EMAIL");
            Assert.That(response7.IsSuccessStatusCode, Is.True);

            var noteResponses3 = await response7.Content.ReadFromJsonAsync<IEnumerable<NoteResponse>>();
            Assert.That(noteResponses3, Is.Not.Null);

            Assert.That(noteResponses3.Count(), Is.EqualTo(2));
            #endregion

            #region get phone mail notes
            var response8 = await _client.GetAsync("/notepad?tags=PHONE&tags=EMAIL");
            Assert.That(response8.IsSuccessStatusCode, Is.True);

            var noteResponses4 = await response8.Content.ReadFromJsonAsync<IEnumerable<NoteResponse>>();
            Assert.That(noteResponses4, Is.Not.Null);

            Assert.That(noteResponses4.Count(), Is.EqualTo(1));
            #endregion

            // cleanup
            await _client.DeleteAsync($"/notepad?noteId={noteId}");
            await _client.DeleteAsync($"/notepad?noteId={noteId2}");
            await _client.DeleteAsync($"/notepad?noteId={noteId3}");
            await _client.DeleteAsync($"/notepad?noteId={noteId4}");
        }

        // Other test methods for UpdateNote, DeleteNote, GetNotes...

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}