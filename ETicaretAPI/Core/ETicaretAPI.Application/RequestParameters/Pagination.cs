 using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.RequestParameters
{
    // Normalde 'record' immutable (değiştirilemez) veri tipleri için kullanılır.
    // Ancak burada set edilebilir property'ler tanımlandığı için aslında class gibi davranır.
    // Eğer sayfalama parametrelerinin değiştirilebilir olması isteniyorsa 'class' kullanmak daha uygundur.
    public record Pagination
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}

