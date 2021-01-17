using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Clients.Wasm.Components.Modals;
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Share.Models.FileStore;
using Benriya.Utils;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.Inventory.Warehouses
{
    public class WarwhouseFormBase : FormBase<Warehouse>
    {
        protected bool IsGridViewFiltered {get;set;}
        protected List<Warehouse_Area> areaList { get; set; }
        protected ModalContent modalForm { get; set; }
        protected ModalConfirm modalDelArea { get; set; }
        protected ModalConfirm modalDelImage { get; set; }
        protected Warehouse_Area ModelAreaData { get; private set; } = new Warehouse_Area(){ id = Guid.NewGuid()};
        protected FileStore_Images ModelImageData { get; set; }
        protected int totalItem { get; set; }
        protected List<string> imgList { get; set; } = new List<string>();
        private bool isEdit { get; set; }
        private Warehouse_Area currentModelData;
        public WarwhouseFormBase()
        {
            model = new Warehouse();
            url = "inventory/Warehouse/";
        }
        protected virtual void OnSearchTextChanged(ChangeEventArgs changeEventArgs, string columnName)
        {            
            string searchText = changeEventArgs.Value.ToString();
            if (model.Areas != null)
                areaList = model.Areas.ToList();           
            Console.WriteLine("Search->: "+searchText);
            areaList = areaList.Where(x => x.name.Contains(searchText)).ToList();      
             IsGridViewFiltered = true;
            
        }

        protected virtual void ShowForm(Warehouse_Area data)
        {
            isEdit = true;
            currentModelData = Set_WH_Data(new Warehouse_Area(),data);            
            ModelAreaData = data;
            modalForm.Open();
        }

        protected async void HandleValidSubmitModal(Warehouse_Area modelData)
        {
            modelData.warehouse_id = model.id;
            ModelAreaData = modelData;
            if (model.Areas == null)
                model.Areas = new List<Warehouse_Area>();
            if (!isEdit)
            {
                model.Areas.Add(ModelAreaData);
                areaList = model.Areas.ToList();
            }
            ModelAreaData = new Warehouse_Area();
            IsGridViewFiltered = true;
            await modalForm.Close();
            
        }
        protected async void OnResetModelModal(Warehouse_Area data)
        {
            SetListSata(data);
            IsGridViewFiltered = false;
            await modalForm.Close();
        }
        protected async void OnCancelModal(Warehouse_Area data)
        {
            SetListSata(data);
            IsGridViewFiltered = false;
            await modalForm.Close();
        }       
        
        private void SetListSata(Warehouse_Area data)
        {
            ModelAreaData = data;
            if (model.Areas != null)
            {
                var areaCollection = model.Areas.ToList();
                var ar = areaCollection.FirstOrDefault(x => x.id == currentModelData.id);
                if (ar != null)
                {
                    ar = Set_WH_Data(ar, currentModelData);
                    model.Areas = areaCollection;
                }
                areaList = areaCollection;
            }
        }

        protected void OpenForm()
        {
            isEdit = false;
            ModelAreaData = new Warehouse_Area() { id = Guid.NewGuid()};
            IsGridViewFiltered = false;
            modalForm.Open();
        }
        protected void OnInsertImage(FileStore_Images file)
        {
            IsGridViewFiltered = false;
            if (model.Images == null)
                model.Images = new List<FileStore_Images>();
            model.Images.Insert(0,file);
            //imgList.Insert(0,file.url+ImagePath.Thumbs+"/"+file.name);
        }

        private Warehouse_Area Set_WH_Data(Warehouse_Area data,Warehouse_Area idata)
        {
            data.id = idata.id;
            data.name = idata.name;
            data.description = idata.description;
            data.created = idata.created;
            data.updated = idata.updated;
            return data;

        }

        protected void DelAreaConfirm(Warehouse_Area data)
        {            
            ModelAreaData = data;
            modalDelArea.Open();
        }

        protected void DelImageConfirm(FileStore_Images data)
        {
            ModelImageData = data;
            modalDelImage.Open();
        }
        protected async void DeleteArea()
        {
            foreach (var area in model.Areas)
            {
                if (area.id == ModelAreaData.id)
                {
                    model.Areas.Remove(area);
                    break;
                }
            }
            areaList = model.Areas.ToList();
            IsGridViewFiltered = true;
            await modalDelArea.Close();
        }

        protected async void DeleteImage()
        {
            foreach (var area in model.Images)
            {
                if (area.id == ModelImageData.id)
                {
                    model.Images.Remove(area);
                    break;
                }
            }
            await modalDelImage.Close();
        }
    }
}
