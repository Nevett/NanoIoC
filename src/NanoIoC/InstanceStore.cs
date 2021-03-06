﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace NanoIoC
{
	/// <summary>
	/// Stores instances for the life of the application
	/// </summary>
	public abstract class InstanceStore : IInstanceStore
    {
		public abstract IDictionary<Type, IList<Tuple<Registration, object>>> Store { get; }
		public abstract IDictionary<Type, IList<Registration>> InjectedRegistrations { get; }
		protected abstract Lifecycle Lifecycle { get; }

		public void Insert(Registration registration, Type type, object instance)
        {
			if(!this.Store.ContainsKey(type))
				this.Store.Add(type, new List<Tuple<Registration, object>>());

			if(registration.InjectionBehaviour == InjectionBehaviour.Override)
				this.Store[type].Clear();

            this.Store[type].Add(new Tuple<Registration, object>(registration, instance));
        }

		public void Inject(Type type, object instance, InjectionBehaviour injectionBehaviour)
		{
			if (!this.InjectedRegistrations.ContainsKey(type))
				this.InjectedRegistrations.Add(type, new List<Registration>());

			var registration = new Registration(type, instance?.GetType() ?? type, null, this.Lifecycle, injectionBehaviour);
			this.InjectedRegistrations[type].Add(registration);

			this.Insert(registration, type, instance);
		}

        public bool ContainsInstancesFor(Type type)
        {
            return this.Store.ContainsKey(type);
        }

		public IEnumerable GetInstances(Type type)
		{
			return this.Store[type];
		}

		public void Clear()
		{
			this.Store.Clear();
			this.InjectedRegistrations.Clear();
		}

		public void RemoveAllInstancesAndRegistrations(Type type)
		{
			// TODO: this should remove all instances from all stores in all httpcontexts :/

			if (this.InjectedRegistrations.ContainsKey(type))
				this.InjectedRegistrations.Remove(type);

			if (this.Store.ContainsKey(type))
				this.Store.Remove(type);
		}

		public void RemoveInstances(Type type)
		{
			if (this.InjectedRegistrations.ContainsKey(type))
				this.InjectedRegistrations.Remove(type);

			if (this.Store.ContainsKey(type))
				this.Store.Remove(type);
		}
    }
}