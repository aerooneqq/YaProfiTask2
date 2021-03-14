using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace YaProfiTask2.Controllers
{
    [ApiController]
    [Route("notes")]
    public class NotesController : ControllerBase
    {
        private class NoteDto
        {
            [JsonProperty("title")] public string NewTitle { get; set; }
            [JsonProperty("content")] public string NewContent { get; set; }


            public void Deconstruct(out string title, out string content)
            {
                title = NewTitle;
                content = NewContent;
            }
        }
        
        private static readonly IList<Note> notes = new List<Note>();
        

        [HttpPost]
        public async Task<IActionResult> CreateNewNote()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync().ConfigureAwait(false);
                (string title, string content) = JsonConvert.DeserializeObject<NoteDto>(body);

                if (content is null)
                {
                    return new BadRequestResult();
                }

                Note newNote = new Note(title, content);
                notes.Add(newNote);
                return new OkObjectResult(newNote);
            }
            catch (Exception ex)
            {
                //logging could be here
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetNote(int id)
        {
            if (notes.FirstOrDefault(note => note.Id == id) is { } foundNote)
            {
                return new OkObjectResult(foundNote);
            }

            return new NotFoundResult();
        }

        [HttpPut("{id:int}")]
        public  async Task<IActionResult> UpdateNote(int id)
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync().ConfigureAwait(false);
                (string newTitle, string newContent) = JsonConvert.DeserializeObject<NoteDto>(body);

                if (notes.FirstOrDefault(note => note.Id == id) is { } foundNote)
                {
                    if (newContent is { })
                    {
                        foundNote.Content = newContent;
                    }

                    if (newTitle is { })
                    {
                        foundNote.Title = newTitle;
                    }

                    return new OkResult();
                }

                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                //logging could be here
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public IActionResult GetNote([FromQuery] string query)
        {
            if (query is null)
            {
                return new OkObjectResult(notes);
            }

            return new OkObjectResult(notes.Where(note => note.Title is { } && note.Title.Contains(query) ||
                                                          note.Content is { } && note.Content.Contains(query)));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteNote(int id)
        {
            if (notes.Remove(notes.FirstOrDefault(n => n.Id == id)))
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
    }
}