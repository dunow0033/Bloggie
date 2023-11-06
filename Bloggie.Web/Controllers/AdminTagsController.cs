﻿using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Repositories;

namespace Bloggie.Web.Controllers;

public class AdminTagsController : Controller
{
    private readonly ITagInterface tagRepository;
    private readonly BloggieDbContext bloggieDbContext;

    public AdminTagsController(BloggieDbContext bloggieDbContext)
    {
        this.bloggieDbContext = bloggieDbContext;
    }


    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };

		bloggieDbContext.Tags.Add(tag);
		bloggieDbContext.SaveChanges();
		//await tagRepository.AddAsync(tag);

		return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult List()
    {
        //var tags = await tagRepository.GetAllAsync();
        var tags = bloggieDbContext.Tags.ToList();

        return View(tags);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);
        //var tags = await tagRepository.GetAllAsync();
        if(tag!= null)
        {
            var editTagRequest = new EditTagRequest
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };

            return View(editTagRequest);
        }

        return View(null);
    }

    [HttpPost]
    public IActionResult Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };

        var existingTag = bloggieDbContext.Tags.Find(tag.Id);

        if(existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;

            bloggieDbContext.SaveChanges();
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);

        if (tag != null)
        {
            bloggieDbContext.Tags.Remove(tag);
            bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}
