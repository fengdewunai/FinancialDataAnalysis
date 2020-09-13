//历史记录Grid
var mainFormGrid = Ext.create('Financial.BaseGrid', {
    region: "center",
    url: "/CompanyPush/GetHistoryGridData",
    autoLoad: false,
    pageSize: 100,
    border: false,
    selModel: { selType: '' }, //选择框
    fields: ["CompanyConnectRecordId", "PhoneConnectDate", "CompanyState", "IsValidateAddress", "ConfirmAddress", "GoHomePerson", "GoHomeTime", "PhoneConnectState","CooperationIntention"],
    columns: [
        { text: '电销联系日期', flex: 1.5, dataIndex: 'PhoneConnectDate', Width: 180, align: 'left' },
        { text: '上门拜访人员', flex: 1.5, dataIndex: 'GoHomePerson', Width: 180, align: 'left' },
        { text: '拜访时间', flex: 1.5, dataIndex: 'GoHomeTime', Width: 180, align: 'left' },
        {
            text: '公司现状', flex: 1.5, dataIndex: 'CompanyState', Width: 180, align: 'left', renderer: function (v) {
                if (v == "1")
                    return '已注销';
                if (v == "2")
                    return '存续';
                if (v == "3")
                    return '待确认';
                return "";

            }
        },
        { text: '操作', flex: 1, dataIndex: 'CompanyConnectRecordId', Width: 120, align: 'center', renderer: function (v) { return "<a href='#' onclick='deleteCompanyRecord(" + v + ")'>删除</a>" } }
    ],
    listeners: {
        "cellclick": function (me, td, cellIndex, record, tr, rowIndex, e, eOpts, a, b, c) {
            mainForm.getForm().findField("CompanyConnectRecordId").setValue(record.data.CompanyConnectRecordId);
            mainForm.getForm().findField("PhoneConnectDate").setValue(record.data.PhoneConnectDate);
            mainForm.getForm().findField("CompanyState").setValue(record.data.CompanyState);
            mainForm.getForm().findField("IsValidateAddress").setValue(record.data.IsValidateAddress);
            mainForm.getForm().findField("ConfirmAddress").setValue(record.data.ConfirmAddress);
            mainForm.getForm().findField("GoHomePerson").setValue(record.data.GoHomePerson);
            mainForm.getForm().findField("GoHomeTime").setValue(record.data.GoHomeTime);
            mainForm.getForm().findField("PhoneConnectState").setValue(record.data.PhoneConnectState);
            mainForm.getForm().findField("CooperationIntention").setValue(record.data.CooperationIntention);
        }
    }
});

mainFormGrid.store.on("load", function (store) {
    if (store.getCount() > 0) {
        mainFormGrid.getSelectionModel().select(0);
        mainFormGrid.fireEvent('cellclick', mainFormGrid, "", 0, store.getAt(0));
    }
}, mainFormGrid);

function deleteCompanyRecord(companyRecordId) {
    Ext.Ajax.request({
        url: '/CompanyPush/DeleteCompanyConnectRecord',
        method: 'post',
        params: {
            companyConnectRecordId: companyRecordId
        },
        success: function (response, options) {
            areaTree.fireEvent('itemclick', _currentTreeNodeData, _currentTreeNodeData);
        },
        failure: function () {
        }
    });
}

Ext.define('mainFormModel', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'CompanyId', mapping: 'CompanyId', type: 'string' },
        { name: 'AreaId', mapping: 'AreaId', type: 'string' },
        { name: 'CompanyName', mapping: 'CompanyName', type: 'string' },
        { name: 'ArtificialPerson', mapping: 'ArtificialPerson', type: 'string' },
        { name: 'SetUpTime', mapping: 'SetUpTime', type: 'date' },
        { name: 'CompanyPhone', mapping: 'CompanyPhone', type: 'string' },
        { name: 'Address', mapping: 'Address', type: 'string' }
    ]
});

var mainForm = new Ext.form.FormPanel({
    title: '公司信息',
    buttonAlign: 'center',
    frame: true,
    region: "center",
    url: '/CompanyPush/AddCompanyRecord',
    reader: new Ext.data.JsonReader({
        root: 'data',//数据的根属性，如果只是纯数据对象，没有什么root，那写""（空串）或者空着就行  
        model: 'mainFormModel'//使用的model 
    }),
    tbar: [
        "->", { text: "新增公司", handler: function () { addCompanyWin.show(); } }
    ],
    items: [{
        columnWidth: .100,
        layout: 'form',
        xtype: 'fieldset',
        style: 'margin-left:10px;margin-right:10px',
        autoHeight: true,
        title: '基本信息',
        labelAlign: 'right',
        items: [{
            xtype: 'hidden',
            name: "CompanyId",
        }, {
            layout: "column",
            frame: true,
            style: 'border:0',
            defaultType: 'textfield',
            items: [{
                xtype: "areacombobox",
                columnWidth: .50,
                fieldLabel: '区域',
                name: "AreaId",
                labelAlign: 'right',
                readOnly: true
            },
            {
                columnWidth: .50,
                fieldLabel: '企业名称',
                name: "CompanyName",
                labelAlign: 'right',
                readOnly: true
            }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [{
                columnWidth: .50,
                fieldLabel: '法人',
                name: "ArtificialPerson",
                labelAlign: 'right',
                readOnly: true
            },
            {
                xtype: "datefield",
                format: 'Y-m-d',
                columnWidth: .50,
                fieldLabel: '成立日期',
                name: "SetUpTime",
                labelAlign: 'right',
                readOnly: true
            }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [{
                columnWidth: .50,
                fieldLabel: '联系电话',
                name: "CompanyPhone",
                labelAlign: 'right',
                readOnly: true
            },
            {
                columnWidth: .50,
                fieldLabel: '住所1',
                name: "Address",
                labelAlign: 'right',
                readOnly: true
            }]
        }]
    }, {
        layout: 'form',
        xtype: 'fieldset',
        style: 'margin-left:10px;margin-right:10px',
        autoHeight: true,
        title: '信息维护',
        labelAlign: 'right',
        items: [{
            xtype: "hidden",
            name: "CompanyConnectRecordId"
        }, {
            layout: "column",
            frame: true,
            style: 'border:0',
            defaultType: 'textfield',
            items: [{
                columnWidth: .50,
                xtype: "fieldcontainer",
                items: [{
                    width: 300,
                    xtype: "datefield",
                    format: 'Y-m-d',
                    fieldLabel: '电销联系日期',
                    name: "PhoneConnectDate",
                    labelAlign: 'right'
                }]
            },
            {
                columnWidth: .50,
                xtype: "fieldcontainer",
                items: [{
                    xtype: 'combo',
                    fieldLabel: '公司现状',
                    labelAlign: 'right',
                    name: 'CompanyState',
                    store: new Ext.data.SimpleStore({
                        fields: ['value', 'text'],
                        data: [
                            ['1', '已注销'],
                            ['2', '存续'],
                            ['3', '待确认'],
                        ]
                    }),
                    displayField: 'text',
                    valueField: 'value',
                    mode: 'local',
                    emptyText: '请选择',
                    readOnly: false
                }]
            }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [
                {
                    columnWidth: 1,
                    xtype: "fieldcontainer",
                    layout: 'hbox',
                    items: [{
                        xtype: 'combo',
                        width: 300,
                        fieldLabel: '是否有效地址',
                        labelAlign: 'right',
                        name: 'IsValidateAddress',
                        store: new Ext.data.SimpleStore({
                            fields: ['value', 'text'],
                            data: [
                                ['1', '有效'],
                                ['2', '无效']
                            ]
                        }),
                        displayField: 'text',
                        valueField: 'value',
                        mode: 'local',
                        emptyText: '请选择',
                        readOnly: false,
                        listeners: {
                            change: function (obj, newValue, oldValue, eOpts) {
                                var value = newValue;
                                if (value == "2") {
                                    mainForm.getForm().findField("ConfirmAddress").show();
                                }
                                else {
                                    mainForm.getForm().findField("ConfirmAddress").setValue("");
                                    mainForm.getForm().findField("ConfirmAddress").hide();
                                }
                            }
                        }
                    },
                    {
                        xtype: "textfield",
                        width: "80%",
                        fieldLabel: '确认地址',
                        name: "ConfirmAddress",
                        labelAlign: 'right',
                        hidden: true
                    }]

                }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [
                {
                    columnWidth: .50,
                    fieldLabel: '上门拜访人员',
                    name: "GoHomePerson",
                    labelAlign: 'right'
                }, {
                    columnWidth: .50,
                    xtype: "fieldcontainer",
                    items: [{
                        xtype: "datefield",
                        format: 'Y-m-d',
                        fieldLabel: '拜访时间',
                        name: "GoHomeTime",
                        labelAlign: 'right'
                    }]
                }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [{
                xtype: 'textarea',
                cols: 7,
                columnWidth: .50,
                fieldLabel: '电话联系情况',
                name: "PhoneConnectState",
                labelAlign: 'right'
            },{
                    columnWidth: .50,
                    xtype: "fieldcontainer",
                    items: [{
                        xtype: 'combo',
                        fieldLabel: '合作意向情况',
                        labelAlign: 'right',
                        name: 'CooperationIntention',
                        store: new Ext.data.SimpleStore({
                            fields: ['value', 'text'],
                            data: [
                                ['1', '无意向'],
                                ['2', '有意向'],
                                ['3', '待确认'],
                            ]
                        }),
                        displayField: 'text',
                        valueField: 'value',
                        mode: 'local',
                        emptyText: '请选择',
                        readOnly: false
                    }]
                }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            buttonAlign: 'center',
            buttons: [{
                xtype: "button",
                width: 50,
                text: "新增",
                handler: function () { SubmitCompanyRecord(1); }
            },
            {
                xtype: "button",
                width: 50,
                text: "修改",
                handler: function () { SubmitCompanyRecord(2); }
            }]
        }]
    }, {
        columnWidth: .100,
        layout: 'border',
        xtype: 'fieldset',
        height: 200,
        title: '历史记录',
        items: [
            mainFormGrid
        ]
    }]
});

function SubmitCompanyRecord(addCompanyTypeId) {
    var form = mainForm.getForm();
    if (form.isValid()) {
        form.submit({
            params: { AddCompanyTypeId: addCompanyTypeId },
            success: function (form, action) {
                mainFormGrid.getStore().load({ params: { companyId: _currentTreeNodeData.raw.id } }); 
                Ext.Msg.alert('提示信息', "操作成功");
            },
            failure: function (form, action) {
                Ext.Msg.alert('提示信息', action.result.restMsg);
            }
        });
    }
}