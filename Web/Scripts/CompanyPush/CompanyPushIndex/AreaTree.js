// 组织结构树
var areaTree = Ext.create('Financial.BaseTree', {
    title: "组织结构",
    id: "AreaTree",
    region: "west",
    width: 300,
    url: "/CompanyPush/GetAreaTreeData",
    //extraParams: { excelId: excelId }
    tbar: [
        "->", { text: "导出数据", handler: ExportExcel }, '|',{ text: "新增地区", handler: function () { addAreaWin.show(); } }
    ],
    listeners: {
        'itemclick': function (node, e) {
            if (e.raw.leaf) {
                _currentTreeNodeData = e;
                mainForm.getForm().reset();
                mainFormGrid.getStore().load({ params: { companyId: e.raw.id } }); 
                mainForm.getForm().load({
                    waitMsg: '正在加载数据请稍后',          //提示信息  
                    waitTitle: '提示',                         //标题  
                    url: '/CompanyPush/GetAreaFormData', //请求的url地址  
                    params: { id: e.raw.id },
                    method: 'POST',              //请求方式  
                    success: function (form, action) { //加载成功的处理函数  
                        
                    },
                    failure: function (form, action) {          //加载失败的处理函数  
                        Ext.Msg.alert('提示', '数据加载失败');
                    }
                });
            }
        }
    }
});

// 导出excel
function ExportExcel() {
    Ext.MessageBox.wait("正在生成Excel", "等待");
    var ids = areaTree.getSelectedNodes().join(',');
    location.href = "/CompanyPush/ExportExcel?ids=" + ids;
    Ext.MessageBox.hide();
}

var addAreaFrom = new Ext.form.FormPanel({
    buttonAlign: 'center',
    frame: true,
    url: '/CompanyPush/AddArea',
    style:'padding-top:20px',
    items: [{
        xtype: "textfield",
        fieldLabel: '地区名称',
        name: "AreaName",
        width:420,
        labelAlign: 'right'
    }]
});

var addAreaWin = Ext.create("Ext.window.Window",
    {
        title: '新增地区',
        modal: true,
        constrainHeader: true,
        buttonAlign: 'center',
        resizable: false,
        height: 150,
        width: 450,
        layout: "fit",
        items: [addAreaFrom],
        closeAction: 'hide',
        buttons: [{
            xtype: "button",
            width: 50,
            text: "保存",
            handler: function () {
                var form = addAreaFrom.getForm();
                if (form.isValid()) {
                    form.submit({
                        success: function (form, action) {
                            Ext.Msg.alert('提示信息', "添加成功");
                            addAreaWin.hide();
                            areaTree.getStore().load();
                            areaComboboxStore.load();
                            //areaTree.getRootNode().reload();
                        },
                        failure: function (form, action) {
                            Ext.Msg.alert('提示信息', action.result.restMsg);
                        }
                    });
                }
            }
        }],
        listeners: {
            show: function () {
                addAreaFrom.getForm().reset();
            }
        }
    });