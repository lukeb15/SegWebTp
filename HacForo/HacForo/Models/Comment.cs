
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace HacForo.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Comment
{

    public int Id { get; set; }

    public string Message { get; set; }

    public System.DateTime CreationDate { get; set; }

    public int UserId { get; set; }

    public int ThreadId { get; set; }



    public virtual User User { get; set; }

    public virtual ForumThread Thread { get; set; }

}

}
