#!/bin/sh

set -e

az aks get-credentials -n william -g dev-space # set the right cluster as kubeconfig

# Apply CRD
kubectl apply -f aad-pod-identity/crd/azureAssignedIdentityCrd.yaml
kubectl apply -f aad-pod-identity/crd/azureIdentityBindingCrd.yaml
kubectl apply -f aad-pod-identity/crd/azureIdentityCrd.yaml

# Deploy Infra
kubectl apply -f aad-pod-identity/deploy/infra/deployment.yaml

#deploy id made in create-azis.sh
kubectl apply -f srcid/aadpodidentity.yaml

#deploy binding
kubectl apply -f srcid/aadpodidentitybinding.yaml