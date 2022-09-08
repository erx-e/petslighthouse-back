﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using petsLighthouseAPI.Models;
using petsLighthouseAPI.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace petsLighthouseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class postpetController : ControllerBase
    {

        private readonly petDBContext _context;
        private readonly IPostPetService _postPetService;

        public postpetController(petDBContext context, IPostPetService postPetService)
        {
            _context = context;
            _postPetService = postPetService;
        }

        [HttpGet]
        [Route("get")]
        public IEnumerable<PostPetView> Get([FromQuery] int? limit = null, int? offset = null)
        {
            if (limit != null && offset != null)
            {
                return _postPetService.getAllPosts(limit, offset);
            }
            return _postPetService.getAllPosts();
        }

        [HttpGet]
        [Route("getByState/{id}")]
        public ActionResult<IEnumerable<PostPetView>> GetByState(string id, [FromQuery] int? limit = null, int? offset = null)
        {
            if (id == null)
            {
                return BadRequest("Must send id state");
            }
            if (limit != null && offset != null)
            {
                return _postPetService.getByState(id, limit, offset);
            }
            return _postPetService.getByState(id);
        }

        [HttpGet]
        [Route("getByFilter")]
        public ActionResult<IEnumerable<PostPetView>> GetByFilter([FromQuery] string? stateId, int? petSpecieId, int? petBreedId, int? provinciaId, int? cantonId, int? sectorId, int? userId, DateTime? date, int? order, int? limit = null, int? offset = null)
        {

            if (limit != null && offset != null)
            {
                var response = _postPetService.getByFilter(stateId, petSpecieId, petBreedId, provinciaId, cantonId, sectorId, userId, date, order, limit, offset);
                if (response.Data != null)
                {
                    return Ok((IEnumerable<PostPetView>)response.Data);
                }
                else
                {
                    return null;
                }
            }
            return Ok((IEnumerable<PostPetView>)_postPetService.getByFilter(stateId, petSpecieId, petBreedId, provinciaId, cantonId, sectorId, userId, date, order).Data);
        }

        [HttpGet]
        [Route("get/{id}")]
        public ActionResult<PostPetView> get([Required] int id)
        {
            var response = _postPetService.getViewPostById(id);
            if (response.Success == 0) return BadRequest();
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("getUpdate/{id}")]
        public ActionResult<UpdatePostPetDTO> getUpdate([Required] int id)
        {
            var response = _postPetService.getPostById(id);
            if (response.Success == 0) return BadRequest();
            return Ok(response.Data);
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Response>> create([Required] CreatePostPetDTO postpetDTO)
        {
            var authR = (Response)HttpContext.Items["User"];
            var authUser = authR.Data as UserView;
            if (authUser.idUser == postpetDTO.idUser)
            {
                var response = await _postPetService.createPostAsync(postpetDTO);
                return Ok(response);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> update([Required] UpdatePostPetDTO postpetDTO)
        {
            var authR = (Response)HttpContext.Items["User"];
            var authUser = authR.Data as UserView;
            if (authUser.idUser == postpetDTO.idUser)
            {
                var response = await _postPetService.updatePostAsync(postpetDTO);
                if (response.Success == 0)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response.Data);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> deleteAsync([Required] int id)
        {
            var authR = (Response)HttpContext.Items["User"];
            var authUser = authR.Data as UserView;
            if (authUser.idUser == id)
            {
                var response = await _postPetService.deletePost(id);
                return Ok(response);
            }
            return Unauthorized();
        }
    }
}
