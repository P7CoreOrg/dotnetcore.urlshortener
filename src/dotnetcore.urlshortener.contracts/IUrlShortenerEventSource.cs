﻿using System;
using System.Threading.Tasks;

namespace dotnetcore.urlshortener.contracts
{
    public interface IUrlShortenerEventSource<T> where T: class
    {
        void AddListenter(EventHandler<T> handler);

        void RemoveListenter(EventHandler<T> handler);

    }

   
}