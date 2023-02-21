﻿using Training.Web.Models;

namespace Training.Web.Services
{
    public class GoodsService : IGoodsService
    {
        public void CheckStorageExpirationDate(RegisteredInvoice invoice)
        {
            bool isGoodsNotOnSaleOrNotSold = invoice.Good.Status != GoodsStatus.Sold && 
                invoice.Good.Status != GoodsStatus.OnSale && 
                invoice.Good.Status != GoodsStatus.Returned;

            bool isStorageTimeExpired = DateTime.Now > invoice.StorageDate;
            if (isStorageTimeExpired && isGoodsNotOnSaleOrNotSold) 
            {
                invoice.Good.Status = GoodsStatus.Expired;
                return;
            }

            if (isGoodsNotOnSaleOrNotSold) 
            {
                invoice.Good.Status = GoodsStatus.Storing;
            }
        }

        public void PutUpOnSale(Good goods)
        {
            goods.Status = GoodsStatus.OnSale;
        }
        public void Sell(Good goods)
        {
            goods.Status = GoodsStatus.Sold;
        }
        public void Return(Good goods)
        {
            if (goods.Status == GoodsStatus.Storing)
            { 
                goods.Status = GoodsStatus.Returned; 
            }
        }
    }
}
