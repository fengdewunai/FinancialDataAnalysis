/**************************************************************************************************
 * Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
 * All rights reserved.
 * Author:Kenshin Cui
 * Date:2013/3/5 23:20:09
 * Description:基于ExtJs4.x的地图组件 
 * Feature：兼容常用浏览器，支持背景设置，提供多种事件交互，支持区域标题、标记、区域颜色自动生成，可导出图片
            支持单个区域多个Path支持,支持点击标记闪烁效果
 * Note:
 * *************************************************************************************************/
//Ext.Loader.setConfig({ enabled: true });
//Ext.Loader.setPath('Ext.ux', '../ux/');
//Ext.require([
//    'Ext.util.*',
//    'Ext.tip.QuickTipManager'
//]);

Ext.draw.engine.ImageExporter.defaultUrl = '/Commons/SVGToImage.do';
Ext.define('BeidaSoft.chart.Map', {
    extend: 'Ext.panel.Panel', //为避免不兼容旧版本ie，这里直接继承于Panel而不是Ext.draw.Component
    alias: 'widget.bdmap',
    layout: 'fit',
    viewBox: false,
    border: false,
    drawCmp: null, //绘图画板，私有变量
    mapDefaultConfig: null,
    map: null, //地图对象,必填项（其中的fill如果要实现过渡动画必须使用十六进制而不能使用别名;name可以使字符串也可以是对象例如{x:80,y:100,fill:null,stroke:null}）形如：var worldmap = {defaults:{},areas:[{name:'', path:'M604.196,161.643l0.514-0.129l0',tip:'',marker{x:20,y:20,title:'',fill:null,stroke:null},fill:null,stroke:null},{name:'',path:'M477.196,261.643l0.514-0.129l0'}]}
    autoMarker: false, //是否自动创建地图标记
    autoName: true, //是否自动创建区域名称
    autoFill: false, //是否自动填充区域背景
    shadow: false, //是否添加区域阴影
    backgroundColor: { from: '#87A2B5', to: '#C8DDE8' }, //背景色，可以为false不设置任何背景色，也可以为单一色字符串，也可以为对象
    clickFlicker: false, //在有标记存在的情况下点击是否闪烁标记
    debug: false, //是否为调试模式
    constructor: function (config) {
        if (config) {
            Ext.apply(this, config);
        }
        this.callParent(arguments);
    },

    initComponent: function () {
        var me = this;
        this.addEvents('mapmouseover', 'mapmouseout', 'mapmousedown', 'mapmouseup'); //定义鼠标移入、移出事件

        //绘图对象初始化
        me.drawCmp = Ext.create('Ext.draw.Component', {
            gradients: [{
                id: 'mapBackgroundGradient',
                angle: 90,
                stops: {
                    0: {
                        color: this.backgroundColor.from
                    },
                    100: {
                        color: this.backgroundColor.to
                    }
                }
            }]
        });
        me.items = me.drawCmp;
        //定义此作用域内全局变量
        this.mapDefaultConfig = {//默认地图配置
            area: {
                fill: '#1C80C0', //[, '#CAD3DB', '#E6EAEE', '#B70000'],//默认填充色列表
                gradual: '#88DEEF', //[, '#E6EAEE', '#DBE67C', '#FC9073'],
                stroke: '#FFFFFF',
                shadowFill: '#AAAAAA', //阴影颜色
                shadowOffset: 2 //阴影偏移量
            },
            marker: {
                fill: '#EA412C',
                stroke: '#FFFFFF'
            },
            areaName: {
                fill: '#000000',
                font: 'bold 12px 微软雅黑'
            },
            autoFills: {//按照区域自动填充为不同颜色的取值
                from: ['#FFE6A6', '#EBFEE0', '#CDEBF5', '#EBFEE0', '#F2F2A8', '#F1C6F3', '#B2FF7F', '#FFC027', '#D7EDFB', '#FCE0EC'],
                to: ['#FFF6D6', '#FBFEE9', '#EDEBF5', '#FBFEF0', '#F2F2D8', '#F1E6F3', '#E2FF7F', '#FFC0C7', '#F7EDFB', '#FCF0EC']
            }
        };
        if (this.map.defaults) {
            Ext.apply(this.mapDefaultConfig, this.map.defaults); //用户配置优先
        }
        this.areaNameOffsetMultiple = 5; //区域标题偏移倍率
        this.callParent(arguments);

    },
    //onRender:function(){
    //    this.callParent(arguments);
    //},
    afterRender: function () {//由于各个精灵使用surface添加，而surface必须渲染后才能使用，因此添加精灵操作放到afterRender中
        var me = this;
        this.callParent(arguments);
        //添加地图事件
        this.drawCmp.surface.on('mousedown', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            //if (mapElementType === 'mapArea' || mapElementType === 'mapAreaName') {
            //    Ext.each(me.map.areas, function (area) {
            //        if ((area.id || area.name) === target.id) {
            //            me.fireEvent('mapmousedown', area, target.getAttribute('mapElementType'), sprite, e, eOpts);
            //            return;
            //        }
            //    });
            //}
            me.fireEvent('mapmousedown', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseup', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseup', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseover', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseover', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseout', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseout', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });

        me.drawBackground();
        //程序主要流程
        for (var i = 0, len = me.map.areas.length; i < len; i += 1) {
            (function (i) {
                var area = me.map.areas[i];
                var areaSprite = me.drawArea(area, i);
                //                var coordinate = me.getAreaCenterCoordinate(areaSprite);
                //                if (!area.marker && me.autoMarker) {
                //                    //自动创建地图标记
                //                    var mk = me.autoName ? { x: coordinate.x, y: (coordinate.y - 8)} : coordinate;
                //                    area.marker = mk;
                //                }
                //                if (area.marker) {
                //                    me.drawMarker(area.marker);
                //                }
                //                if (me.autoName) {
                //                    var aName;
                //                    if (typeof area.name == 'string') {//area.name可以使字符串也可以配置为对象
                //                        aName = { text: area.name, x: coordinate.x - area.name.length * me.areaNameOffsetMultiple, y: me.autoMarker ? (coordinate.y + 8) : coordinate.y };
                //                    } else {
                //                        aName = me.name;
                //                    }
                //                    me.drawAreaName(aName);
                //                }
            } (i));
        }
        //this.createContextMenu();//创建右键菜单
    },
    onDestroy: function () {
        delete this.mapDefaultConfig;
        delete this.drawCmp;
        delete this.flickerRunner;
    },
    //createContextMenu: function () {
    //    var mapMenu = Ext.create('Ext.menu.Menu', {
    //        items: [{
    //            text: '保存为PNG图片'
    //        }
    //        //,{
    //        //    text: '打印'
    //        //}
    //        ]
    //    });

    //},
    getAutoFill: function (index) {
        var fillMap = [];
        var i = index;
        if (index < this.mapDefaultConfig.autoFills.from.length) {
            i = index;
        } else {
            i = index % this.mapDefaultConfig.autoFills.from.length;
        }
        fillMap[0] = this.mapDefaultConfig.autoFills.from[i];
        fillMap[1] = this.mapDefaultConfig.autoFills.to[i];
        return fillMap;
    },
    getAreaCenterCoordinate: function (areaSprite) {//私有方法，取得区域中心坐标
        var bbox = areaSprite.getBBox();
        return { x: bbox.x + bbox.width / 2, y: bbox.y + bbox.height / 2 };
    },
    areaMouseover: function (area, toColor, fromColor, spriteComposite, e, t, eOpts) {//私有方法，鼠标移入动画
        var me = this;
        spriteComposite.stopAnimation();
        spriteComposite.setAttributes({
            scale: {
                x: 1.01,
                y: 1.01
            }
        }, true);
        for (var i = 0, len = spriteComposite.getCount(); i < len; i += 1) {//颜色渐变只影响区域不处理阴影、名称和标记
            if ((me.shadow && i == 1) || (!me.shadow && i == 0)) {
                spriteComposite.getAt(i).animate({ to: { fill: (me.autoFill ? toColor : me.mapDefaultConfig.area.gradual) }, duration: 500 }); //from: { fill: area.fill || mapDefaultConfig.area.fill }, 
            }
        }
    },
    areaMouseout: function (area, toColor, fromColor, spriteComposite, e, t, eOpts) {//私有方法，鼠标移出动画
        var me = this;
        spriteComposite.stopAnimation();
        spriteComposite.setAttributes({
            scale: {
                x: 1,
                y: 1
            }
        }, true);
        for (var i = 0, len = spriteComposite.getCount(); i < len; i += 1) {
            if ((me.shadow && i == 1) || (!me.shadow && i == 0)) {
                spriteComposite.getAt(i).animate({ to: { fill: area.fill || (me.autoFill ? fromColor : me.mapDefaultConfig.area.fill) }, duration: 300 });
            }
        }
    },
    areaMouseup: function (area, toColor, fromColor, spriteComposite, e) {
        var me = this;
        if (me.clickFlicker) {
            for (var i = 0, len = spriteComposite.getCount(); i < len; i += 1) {
                if ((me.shadow && i == 2) || (!me.shadow && i == 1)) {
                    me.markerFlicker(spriteComposite.getAt(i));
                }
            }
        }
    },
    markerFlicker: function (markerSprite) {//私有方法，闪烁标记
        if (this.flickerRunner) {
            this.flickerRunner.destroy();
        }
        this.flickerRunner = new Ext.util.TaskRunner();
        var task = this.flickerRunner.start({
            run: function () {
                markerSprite.setAttributes({
                    radius: 6
                }, true);
                markerSprite.animate({
                    to: {
                        /*scale: {
                        x: 2,
                        y: 2
                        }*/
                        radius: 4
                    },
                    duration: 500
                });
            },
            interval: 800
        });
    },
    drawBackground: function () {//绘制背景，私有方法,rect精灵不兼容非ie浏览器
        var me = this;
        var groupID = me.id + '_map';
        var bgFill = 'url(#mapBackgroundGradient)';
        if (typeof me.backgroundColor == 'string') {
            bgFill = me.backgroundColor;
        }
        var bgSprite;
        if (Ext.supports.Svg) {
            bgSprite = Ext.create('Ext.draw.Sprite', {
                id: 'mapBackground',
                type: 'rect',
                width: me.width,
                height: me.height,
                fill: bgFill,
                group: groupID
            });
        } else {
            bgSprite = Ext.create('Ext.draw.Sprite', {
                id: 'mapBackground',
                type: 'image',
                //src: 'images/bg_blue.gif',
                src: '/modules/commons/js/ext-4.2/ux/images/bg_blue.gif',
                width: me.width,
                height: me.height,
                group: groupID
            });
        }
        me.drawCmp.surface.add(bgSprite);
        bgSprite.show(true);
        Ext.getDom(bgSprite.id).setAttribute('mapElementType', 'mapBackground'); //设置对应dom的自定义属性以区别精灵类型
        return bgSprite;
    },
    addAreaTip: function (area, sprt) {//添加区域提示，私有方法
        Ext.create('Ext.tip.ToolTip', {
            target: sprt.id,
            trackMouse: true,
            html: area.tip
        });
    },
    drawAreaName: function (areaName) {//绘制地图标记，私有方法
        var me = this;
        var groupID = me.id + '_map';
        var areaNameSprite = Ext.create('Ext.draw.Sprite', {
            type: 'text',
            text: areaName.text,
            mapElementType: 'mapAreaName',
            x: areaName.x,
            y: areaName.y,
            fill: areaName.fill || me.mapDefaultConfig.areaName.fill,
            font: areaName.font || me.mapDefaultConfig.areaName.font,
            group: groupID
        });
        me.drawCmp.surface.add(areaNameSprite);
        areaNameSprite.show(true);
        Ext.getDom(areaNameSprite.id).setAttribute('mapElementType', 'mapAreaName');
        return areaNameSprite;
    },
    drawMarker: function (marker) {//绘制地图标记，私有方法
        var me = this;
        var groupID = me.id + '_marker';
        var markerSprite = Ext.create('Ext.draw.Sprite', {
            type: 'circle',
            radius: 4,
            x: marker.x,
            y: marker.y,
            fill: marker.fill || me.mapDefaultConfig.marker.fill,
            stroke: marker.stroke || me.mapDefaultConfig.marker.stroke,
            'stroke-width': 2,
            group: groupID
        });
        me.drawCmp.surface.add(markerSprite);
        markerSprite.show(true);
        Ext.getDom(markerSprite.id).setAttribute('mapElementType', 'mapMarker');
        return markerSprite;
    },
    drawArea: function (area, index) {
        var me = this, fromColor = me.getAutoFill(index)[0], toColor = me.getAutoFill(index)[1], groupID = (area.id || area.name);
        var pathCount = Ext.isArray(area.path) ? area.path.length : 1;
        area.path = (pathCount === 1) ? [area.path] : area.path;
        var spriteGroup = Ext.create('Ext.draw.CompositeSprite', {
            id: (area.id || area.name) + '_composite',
            surface: me.drawCmp.surface//目前不起作用
        });
        for (var i = 0; i < pathCount; ++i) {
            (function (i) {
                //添加阴影
                var id = (area.id || area.name) + '_' + i;
                if (me.shadow) {
                    var areaSpriteShadow = Ext.create('Ext.draw.Sprite', {
                        id: id + '_shadow',
                        type: 'path',
                        path: area.path[i],
                        fill: me.mapDefaultConfig.area.shadowFill,
                        group: groupID
                    });
                    areaSpriteShadow.setAttributes({//移动区域绘制阴影
                        translate: {
                            x: me.mapDefaultConfig.area.shadowOffset,
                            y: me.mapDefaultConfig.area.shadowOffset
                        }
                    }, true);
                    me.drawCmp.surface.add(areaSpriteShadow);
                    spriteGroup.add(areaSpriteShadow);
                }
                //创建区域
                var areaSprite = Ext.create('Ext.draw.Sprite', {
                    id: id, //没有id则直接使用name作为id
                    type: 'path',
                    path: area.path[i],
                    fill: area.fill || (me.autoFill ? fromColor : me.mapDefaultConfig.area.fill),
                    'stroke-width': 1,
                    stroke: area.stroke || me.mapDefaultConfig.area.stroke,
                    group: groupID
                });
                if (me.debug) {//由于mouseup事件目前在SpriteComposite中暂无eOpts参数，暂时不委托此操作
                    areaSprite.on('mouseup', function (e,eOpts) {
                        console.log('x:' + eOpts.getX() + '\r\ny:' + eOpts.getY());
                    });
                }
                //添加提示
                if (area.tip) {
                    areaSprite.on('render', function (sprt, eOpts) {
                        me.addAreaTip(area, areaSprite);
                    });
                }
                me.drawCmp.surface.add(areaSprite); //SpriteComposite目前只能用于管理一组sprite还无法直接渲染，仍然需要添加到surface中
                spriteGroup.add(areaSprite);
                //添加地图标记和区域名称
                if (i === 0) {//如果有多个路径以第一个路径为准
                    var coordinate = me.getAreaCenterCoordinate(areaSprite);
                    if (!area.marker && me.autoMarker) {
                        //自动创建地图标记
                        var mk = me.autoName ? { x: coordinate.x, y: (coordinate.y - 8)} : coordinate;
                        area.marker = mk;
                    }
                    if (area.marker) {
                        var marker = me.drawMarker(area.marker);
                        spriteGroup.add(marker);
                    }
                    if (me.autoName) {
                        var aName;
                        if (typeof area.name == 'string') {//area.name可以使字符串也可以配置为对象
                            aName = { text: area.name, x: coordinate.x - area.name.length * me.areaNameOffsetMultiple, y: me.autoMarker ? (coordinate.y + 8) : coordinate.y };
                        } else {
                            aName = me.name;
                        }
                        spriteGroup.add(me.drawAreaName(aName));
                    }
                }
            } (i));
        }
        spriteGroup.show(true);
        //定义区域属性
        for (var i = 0, len = spriteGroup.getCount(); i < len; i += 1) {
            Ext.getDom(spriteGroup.getAt(i).id).setAttribute('mapElementType', 'mapArea'); //设置地图元素类型
            Ext.getDom(spriteGroup.getAt(i).id).setAttribute('areaIndex', index); //设置区域路径索引
        }
        //添加互动动画
        spriteGroup.on('mouseover', function (e, t, eOpts) {
            me.areaMouseover(area, toColor, fromColor, this, e, t, eOpts);
        });
        spriteGroup.on('mouseout', function (e, t, eOpts) {
            me.areaMouseout(area, toColor, fromColor, this, e, t, eOpts);
        });
        spriteGroup.on('mouseup', function (e) {
            me.areaMouseup(area, toColor, fromColor, this, e);
        });
        return spriteGroup.first(); //返还区域中第一个路径
    },
    print: function () {//打印

    },
    save: function () {//保存成图片
        Ext.draw.Surface.save(this.drawCmp.surface, {
            type: 'image/png'
        });
    }
});