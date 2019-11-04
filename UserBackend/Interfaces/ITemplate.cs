using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserBackend.Interfaces
{
    interface ITemplateCrud<T>
    {
        public IActionResult GetAll();

        public IActionResult Get(int id);

        public IActionResult Create([FromBody] T item);

        public IActionResult Edit(int id, [FromBody] T item);

        public IActionResult Delete(int id);
    }
}
